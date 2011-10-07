using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Inbox2.Framework.UI.Controls
{
	/// <summary>
	/// VirtualizingTabPanel
	/// </summary>
	public class VirtualizingTabPanel : VirtualizingPanel, IScrollInfo
	{
		private int _maxVisibleItems = 0;
		private double _maxChildWidthOrHeight = 0;
		private List<Rect> _childRects;
		private Dock _tabStripPlacement;

		// scrolling
		private TranslateTransform _translateTransform = new TranslateTransform();
		private bool _canHScroll = false;
		private bool _canVScroll = false;
		private Size _extent = new Size(0, 0);
		private Size _oldExtent = new Size(0, 0);
		private Size _viewPort = new Size(0, 0);
		private Size _lastSize = new Size(0, 0);
		private Point _offset = new Point(0, 0);
		private ScrollViewer _scrollOwner;

		private int _firstVisibleIndex = 0;
		private int _actualChildCount = 0;

		public VirtualizingTabPanel()
		{
			_childRects = new List<Rect>(4);
			base.RenderTransform = _translateTransform;
		}

		#region CLR Properties
		private Dock TabStripPlacement
		{
			get
			{
				Dock dock = Dock.Top;
				TabControl templatedParent = base.TemplatedParent as TabControl;
				if (templatedParent != null)
				{
					dock = templatedParent.TabStripPlacement;
				}
				return dock;
			}
		}
		private double MinimumChildWidth
		{
			get
			{
				TabControl templatedParent = base.TemplatedParent as TabControl;
				if (templatedParent != null)
					return templatedParent.TabItemMinWidth;
				return 0;
			}
		}
		private double MinimumChildHeight
		{
			get
			{
				TabControl templatedParent = base.TemplatedParent as TabControl;
				if (templatedParent != null)
					return templatedParent.TabItemMinHeight;
				return 0;
			}
		}
		private double MaximumChildWidth
		{
			get
			{
				TabControl templatedParent = base.TemplatedParent as TabControl;
				if (templatedParent != null)
					return templatedParent.TabItemMaxWidth;
				return double.PositiveInfinity;
			}
		}
		private double MaximumChildHeight
		{
			get
			{
				TabControl templatedParent = base.TemplatedParent as TabControl;
				if (templatedParent != null)
					return templatedParent.TabItemMaxHeight;
				return double.PositiveInfinity;
			}
		}

		private int FirstVisibleIndex
		{
			get { return _firstVisibleIndex; }
			set
			{
				if (value < 0)
				{
					_firstVisibleIndex = 0;
					return;
				}

				_firstVisibleIndex = value;
				if (LastVisibleIndex > _actualChildCount - 1)
					FirstVisibleIndex--;
			}
		}

		private int LastVisibleIndex
		{
			get { return _firstVisibleIndex + _maxVisibleItems - 1; }
		}
		#endregion

		#region Dependancy Properties

		/// <summary>
		/// CanScrollLeftOrUp Dependancy Property
		/// </summary>
		[Browsable(false)]
		internal bool CanScrollLeftOrUp
		{
			get { return (bool)GetValue(CanScrollLeftOrUpProperty); }
			set { SetValue(CanScrollLeftOrUpProperty, value); }
		}
		internal static readonly DependencyProperty CanScrollLeftOrUpProperty = DependencyProperty.Register("CanScrollLeftOrUp", typeof(bool), typeof(VirtualizingTabPanel), new UIPropertyMetadata(false));

		/// <summary>
		/// CanScrollRightOrDown Dependancy Property
		/// </summary>
		[Browsable(false)]
		internal bool CanScrollRightOrDown
		{
			get { return (bool)GetValue(CanScrollRightOrDownProperty); }
			set { SetValue(CanScrollRightOrDownProperty, value); }
		}
		internal static readonly DependencyProperty CanScrollRightOrDownProperty = DependencyProperty.Register("CanScrollRightOrDown", typeof(bool), typeof(VirtualizingTabPanel), new UIPropertyMetadata(false));

		#endregion

		/// <summary>
		/// OnItemsChanged override
		/// </summary>
		protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
		{
			ItemsControl ic = ItemsControl.GetItemsOwner(this);
			if (ic == null) return;

			_actualChildCount = ic.Items.Count;

			switch (args.Action)
			{
				case NotifyCollectionChangedAction.Remove:
				case NotifyCollectionChangedAction.Replace:
				case NotifyCollectionChangedAction.Move:
					RemoveInternalChildRange(args.Position.Index, args.ItemUICount);

					// if we've removed the last tabitem, we need to scroll to the left
					while (FirstVisibleIndex > 0 && LastVisibleIndex > ic.Items.Count - 1)
						FirstVisibleIndex--;
					break;
			}
		}

		#region MeasureOverride
		/// <summary>
		/// Measure Override
		/// </summary>
		protected override Size MeasureOverride(Size availableSize)
		{
			_viewPort = availableSize;
			_tabStripPlacement = TabStripPlacement;

			switch (_tabStripPlacement)
			{
				case Dock.Top:
				case Dock.Bottom:
					return MeasureHorizontal(availableSize);

				case Dock.Left:
				case Dock.Right:
					return MeasureVertical(availableSize);
			}

			return new Size();
		}
		#endregion

		#region MeasureHorizontal
		/// <summary>
		/// Measure the tab items for docking at the top or bottom
		/// </summary>
		private Size MeasureHorizontal(Size availableSize)
		{
			ItemsControl ic = ItemsControl.GetItemsOwner(this);
			if (ic == null || ic.Items.Count == 0)
				return new Size();

			_maxChildWidthOrHeight = 0;
			_actualChildCount = ic.Items.Count;
			EnsureChildRects(_actualChildCount);


			double extentWidth = 0;
			double[] widths = new double[ic.Items.Count];  // stores the widths of the items for use in the arrange pass

			// we will first measure all the children with unlimited space to get their desired sizes
			// this will also get us the height required for all TabItems
			for (int i = 0; i < ic.Items.Count; i++)
			{
				FrameworkElement child = ic.Items[i] as FrameworkElement;
				if (child == null) return new Size();

				// reset any width/height constraints on the child, 
				// as the autosizing function does not support them
				child.BeginInit();
				child.Width = double.NaN;
				child.MaxWidth = double.PositiveInfinity;
				child.MinWidth = 0;

				child.Height = double.NaN;
				child.MaxHeight = double.PositiveInfinity;
				child.MinHeight = 0;
				child.EndInit();

				child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

				// calculate the maximum child height
				_maxChildWidthOrHeight = Math.Max(_maxChildWidthOrHeight, Math.Ceiling(child.DesiredSize.Height));

				// calculate the child width while respecting the Maximum & Minimum width constraints
				widths[i] = Math.Min(MaximumChildWidth, Math.Max(MinimumChildWidth, Math.Ceiling(child.DesiredSize.Width)));

				// determines how much horizontal space we require
				extentWidth += widths[i];
			}
			_maxChildWidthOrHeight = Math.Max(MinimumChildHeight, Math.Min(MaximumChildHeight, _maxChildWidthOrHeight));  // observe the constraints
			_extent = new Size(extentWidth, _maxChildWidthOrHeight);

			bool flag = false;
			// 1). all the children fit into the available space using there desired widths
			if (extentWidth <= availableSize.Width)
			{
				_maxVisibleItems = ic.Items.Count;
				FirstVisibleIndex = 0;

				double left = 0;
				for (int i = 0; i < _actualChildCount; i++)
				{
					_childRects[i] = new Rect(left, 0, widths[i], _maxChildWidthOrHeight);
					left += widths[i];
				}

				CanScrollLeftOrUp = false;
				CanScrollRightOrDown = false;

				flag = true;
			}

			// 2). all the children fit in the available space if we reduce their widths to a uniform value 
			// while staying within the MinimumChildWidth and MaximumChildWidth constraints
			if (!flag)
			{
				// make sure the width is not greater than the MaximumChildWidth constraints
				double targetWidth = Math.Min(MaximumChildWidth, availableSize.Width / _actualChildCount);

				// target width applies now if whether we can fit all items in the available space or whether we are scrolling
				if (targetWidth >= MinimumChildWidth)
				{
					_maxVisibleItems = ic.Items.Count;
					FirstVisibleIndex = 0;

					extentWidth = 0;
					double left = 0;

					for (int i = 0; i < _actualChildCount; i++)
					{
						extentWidth += targetWidth;
						widths[i] = targetWidth;
						_childRects[i] = new Rect(left, 0, widths[i], _maxChildWidthOrHeight);
						left += widths[i];
					}
					_extent = new Size(extentWidth, _maxChildWidthOrHeight);

					flag = true;

					CanScrollLeftOrUp = false;
					CanScrollRightOrDown = false;

				}
			}

			// 3) we can not fit all the children in the viewport, so now we will enable scrolling/virtualizing items
			if (!flag)
			{
				_maxVisibleItems = (int)Math.Floor(_viewPort.Width / MinimumChildWidth);            // calculate how many visible children we can show at once
				double targetWidth = availableSize.Width / _maxVisibleItems;                        // calculate the new target width
				FirstVisibleIndex = _firstVisibleIndex;

				extentWidth = 0;
				double left = 0;
				for (int i = 0; i < _actualChildCount; i++)
				{
					extentWidth += targetWidth;
					widths[i] = targetWidth;

					_childRects[i] = new Rect(left, 0, widths[i], _maxChildWidthOrHeight);
					left += widths[i];
				}
				_extent = new Size(extentWidth, _maxChildWidthOrHeight);

				CanScrollLeftOrUp = LastVisibleIndex < _actualChildCount - 1;
				CanScrollRightOrDown = FirstVisibleIndex > 0;
			}

			// Realize the visible items

			UIElementCollection children = this.InternalChildren;
			IItemContainerGenerator generator = this.ItemContainerGenerator;

			GeneratorPosition pos = generator.GeneratorPositionFromIndex(FirstVisibleIndex);
			int childIndex = (pos.Offset == 0) ? pos.Index : pos.Index + 1;

			using (generator.StartAt(pos, GeneratorDirection.Forward, true))
			{
				for (int itemIndex = FirstVisibleIndex; itemIndex <= LastVisibleIndex; itemIndex++, ++childIndex)
				{
					bool newlyRealized = false;

					UIElement child = generator.GenerateNext(out newlyRealized) as UIElement;

					if (newlyRealized)
					{
						if (childIndex >= children.Count)
							base.AddInternalChild(child);
						else
							base.InsertInternalChild(childIndex, child);

						generator.PrepareItemContainer(child);
					}
					child.Measure(new Size(widths[itemIndex], _maxChildWidthOrHeight));
				}
			}

			// virtualize items that are no longer visible
			CleanUpItems();

			return new Size(availableSize.Width, _maxChildWidthOrHeight);
		}
		#endregion

		#region MeasureVertical

		/// <summary>
		/// Measure the tab items for docking at the left or right
		/// </summary>
		private Size MeasureVertical(Size availableSize)
		{
			ItemsControl ic = ItemsControl.GetItemsOwner(this);
			if (ic == null || ic.Items.Count == 0)
				return new Size();

			_maxChildWidthOrHeight = 0;
			_actualChildCount = ic.Items.Count;
			EnsureChildRects(_actualChildCount);

			double extentHeight = 0;
			double[] heights = new double[ic.Items.Count];

			// we will first measure all the children with unlimited space to get their desired sizes
			// this will also get us the height required for all TabItems
			for (int i = 0; i < ic.Items.Count; i++)
			{
				FrameworkElement child = ic.Items[i] as FrameworkElement;
				if (child == null) return new Size();

				// reset any width/height constraints on the child, 
				// as the autosizing function does not support them
				child.BeginInit();
				child.Width = double.NaN;
				child.MaxWidth = double.PositiveInfinity;
				child.MinWidth = 0;

				child.Height = double.NaN;
				child.MaxHeight = double.PositiveInfinity;
				child.MinHeight = 0;
				child.EndInit();

				child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

				// calculate the maximum child width
				_maxChildWidthOrHeight = Math.Max(_maxChildWidthOrHeight, Math.Ceiling(child.DesiredSize.Width));

				// calculate the child width while respecting the Maximum & Minimum width constraints
				heights[i] = Math.Min(MaximumChildHeight, Math.Max(MinimumChildHeight, Math.Ceiling(child.DesiredSize.Height)));

				// determines how much horizontal space we require
				extentHeight += heights[i];
			}
			_maxChildWidthOrHeight = Math.Max(MinimumChildWidth, Math.Min(MaximumChildWidth, _maxChildWidthOrHeight));  // observe the constraints
			_extent = new Size(_maxChildWidthOrHeight, extentHeight);

			bool flag = false;
			// 1). all the children fit into the available space using there desired widths
			if (extentHeight <= availableSize.Height)
			{
				_maxVisibleItems = ic.Items.Count;
				FirstVisibleIndex = 0;

				double top = 0;
				for (int i = 0; i < _actualChildCount; i++)
				{
					_childRects[i] = new Rect(0, top, _maxChildWidthOrHeight, heights[i]);
					top += heights[i];
				}

				CanScrollLeftOrUp = false;
				CanScrollRightOrDown = false;

				flag = true;
			}

			// 2). all the children fit in the available space if we reduce their widths to a uniform value 
			// while staying within the MinimumChildWidth and MaximumChildWidth constraints
			if (!flag)
			{
				// make sure the width is not greater than the MaximumChildWidth constraints
				double targetHeight = Math.Min(MaximumChildHeight, availableSize.Height / _actualChildCount);

				// target width applies now if whether we can fit all items in the available space or whether we are scrolling
				if (targetHeight >= MinimumChildHeight)
				{
					_maxVisibleItems = ic.Items.Count;
					FirstVisibleIndex = 0;

					extentHeight = 0;
					double top = 0;

					for (int i = 0; i < _actualChildCount; i++)
					{
						extentHeight += targetHeight;
						heights[i] = targetHeight;
						_childRects[i] = new Rect(0, top, _maxChildWidthOrHeight, heights[i]);
						top += heights[i];
					}
					_extent = new Size(_maxChildWidthOrHeight, extentHeight);

					flag = true;

					CanScrollLeftOrUp = false;
					CanScrollRightOrDown = false;

				}
			}

			// 3) we can not fit all the children in the viewport, so now we will enable scrolling/virtualizing items
			if (!flag)
			{
				_maxVisibleItems = (int)Math.Floor(_viewPort.Height / MinimumChildHeight);          // calculate how many visible children we can show at once
				double targetHeight = availableSize.Height / _maxVisibleItems;                      // calculate the new target width
				FirstVisibleIndex = _firstVisibleIndex;

				extentHeight = 0;
				double top = 0;
				for (int i = 0; i < _actualChildCount; i++)
				{
					extentHeight += targetHeight;
					heights[i] = targetHeight;
					_childRects[i] = new Rect(0, top, _maxChildWidthOrHeight, heights[i]);
					top += heights[i];
				}
				_extent = new Size(_maxChildWidthOrHeight, extentHeight);

				CanScrollLeftOrUp = LastVisibleIndex < _actualChildCount - 1;
				CanScrollRightOrDown = FirstVisibleIndex > 0;
			}

			UIElementCollection children = this.InternalChildren;
			IItemContainerGenerator generator = this.ItemContainerGenerator;

			// Realize the visible items
			GeneratorPosition pos = generator.GeneratorPositionFromIndex(FirstVisibleIndex);
			int childIndex = (pos.Offset == 0) ? pos.Index : pos.Index + 1;

			using (generator.StartAt(pos, GeneratorDirection.Forward, true))
			{
				for (int itemIndex = FirstVisibleIndex; itemIndex <= LastVisibleIndex; itemIndex++, ++childIndex)
				{
					bool newlyRealized = false;

					UIElement child = generator.GenerateNext(out newlyRealized) as UIElement;

					if (newlyRealized)
					{
						if (childIndex >= children.Count)
							base.AddInternalChild(child);
						else
							base.InsertInternalChild(childIndex, child);

						generator.PrepareItemContainer(child);
					}
					child.Measure(new Size(_maxChildWidthOrHeight, heights[itemIndex]));
				}
			}

			// virtualize items that are no longer visible
			CleanUpItems();

			return new Size(_maxChildWidthOrHeight, availableSize.Height);
		}
		#endregion

		#region ArrangeOverride

		/// <summary>
		/// Arrange Override
		/// </summary>
		protected override Size ArrangeOverride(Size finalSize)
		{
			// monitors changes to the ScrollViewer extent value
			if (_oldExtent != _extent)
			{
				_oldExtent = _extent;
				if (_scrollOwner != null)
					_scrollOwner.InvalidateScrollInfo();
			}

			// monitors changes to the parent container size, (ie window resizes)
			if (finalSize != _lastSize)
			{
				_lastSize = finalSize;
				if (_scrollOwner != null)
					_scrollOwner.InvalidateScrollInfo();
			}

			// monitor scrolling being removed
			bool invalidateMeasure = false;
			if (_extent.Width <= _viewPort.Width && _offset.X > 0)
			{
				_offset.X = 0;
				_translateTransform.X = 0;

				if (_scrollOwner != null)
					_scrollOwner.InvalidateScrollInfo();

				invalidateMeasure = true;
			}
			if (_extent.Height <= _viewPort.Height && _offset.Y > 0)
			{
				_offset.Y = 0;
				_translateTransform.Y = 0;

				if (_scrollOwner != null)
					_scrollOwner.InvalidateScrollInfo();

				invalidateMeasure = true;
			}
			if (invalidateMeasure)
				InvalidateMeasure();



			// arrange the children
			double leftOrTop = 0;
			for (int i = 0; i < InternalChildren.Count; i++)
			{
				UIElement child = InternalChildren[i];

				GeneratorPosition childGeneratorPos = new GeneratorPosition(i, 0);
				int itemIndex = this.ItemContainerGenerator.IndexFromGeneratorPosition(childGeneratorPos);

				child.Arrange(_childRects[itemIndex]);

				leftOrTop += (_tabStripPlacement == Dock.Top || _tabStripPlacement == Dock.Bottom) ? _childRects[i].Width : _childRects[i].Height;
			}


			// we need these lines as when the Scroll Buttons get Shown/Hidden,
			// the _offset value gets out of line, this will ensure that our scroll position stays in line
			if (InternalChildren.Count > 0)
			{
				_offset = _childRects[FirstVisibleIndex].TopLeft;
				_translateTransform.X = -_offset.X;
				_translateTransform.Y = -_offset.Y;
			}

			return finalSize;
		}
		#endregion

		/// <summary>
		/// Virtualize hidden items
		/// </summary>
		private void CleanUpItems()
		{
			UIElementCollection children = this.InternalChildren;
			IItemContainerGenerator generator = this.ItemContainerGenerator;

			for (int i = children.Count - 1; i >= 0; i--)
			{
				GeneratorPosition childGeneratorPos = new GeneratorPosition(i, 0);
				int itemIndex = generator.IndexFromGeneratorPosition(childGeneratorPos);

				if (itemIndex == -1)
					continue;

				if (itemIndex < FirstVisibleIndex || itemIndex > LastVisibleIndex)
				{
					generator.Remove(childGeneratorPos, 1);
					RemoveInternalChildRange(i, 1);
				}
			}
		}

		private void EnsureChildRects(int items)
		{
			while (items > _childRects.Count)
				_childRects.Add(new Rect());
		}

		#region IScrollInfo Members

		public bool CanHorizontallyScroll
		{
			get { return _canHScroll; }
			set { _canHScroll = value; }
		}

		public bool CanVerticallyScroll
		{
			get { return _canVScroll; }
			set { _canVScroll = value; }
		}

		public double ExtentHeight
		{
			get { return _extent.Height; }
		}

		public double ExtentWidth
		{
			get { return _extent.Width; }
		}

		public double HorizontalOffset
		{
			get { return _offset.X; }
		}

		public void LineDown()
		{
			LineRight();
		}

		public void LineLeft()
		{
			// this works because we can guarantee that when we are in scroll mode, 
			// there will be children, and they will all be of equal size
			FirstVisibleIndex++;

			if (_tabStripPlacement == Dock.Top || _tabStripPlacement == Dock.Bottom)
				SetHorizontalOffset(HorizontalOffset + _childRects[0].Width);
			else
				SetVerticalOffset(HorizontalOffset + _childRects[0].Height);
		}

		public void LineRight()
		{
			FirstVisibleIndex--;

			if (_tabStripPlacement == Dock.Top || _tabStripPlacement == Dock.Bottom)
				SetHorizontalOffset(HorizontalOffset - _childRects[0].Width);
			else
				SetVerticalOffset(HorizontalOffset - _childRects[0].Height);
		}

		public void LineUp()
		{
			LineLeft();
		}

		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			ItemsControl ic = ItemsControl.GetItemsOwner(this);
			if (ic == null) return Rect.Empty;

			int index = -1;
			for (int i = 0; i < ic.Items.Count; i++)
			{
				if (visual.Equals(ic.Items[i]))
				{
					index = i;
					break;
				}
			}
			if (index > -1)
			{
				if (index < FirstVisibleIndex)
					FirstVisibleIndex = index;
				else if (index > LastVisibleIndex)
				{
					while (index > LastVisibleIndex)
						FirstVisibleIndex++;
				}

				InvalidateMeasure();
				UpdateLayout();
			}

			return Rect.Empty;
		}

		public void MouseWheelDown()
		{
			LineDown();
		}

		public void MouseWheelLeft()
		{
			LineLeft();
		}

		public void MouseWheelRight()
		{
			LineRight();
		}

		public void MouseWheelUp()
		{
			LineUp();
		}

		public void PageDown()
		{
			throw new NotImplementedException();
		}

		public void PageLeft()
		{
			throw new NotImplementedException();
		}

		public void PageRight()
		{
			throw new NotImplementedException();
		}

		public void PageUp()
		{
			throw new NotImplementedException();
		}

		public ScrollViewer ScrollOwner
		{
			get { return _scrollOwner; }
			set { _scrollOwner = value; }
		}

		public void SetHorizontalOffset(double offset)
		{
			if (offset < 0 || _viewPort.Width >= _extent.Width)
				offset = 0;
			else
			{
				if (offset + _viewPort.Width > _extent.Width)
					offset = _extent.Width - _viewPort.Width;
			}

			_offset.X = offset;
			if (_scrollOwner != null)
				_scrollOwner.InvalidateScrollInfo();

			_translateTransform.X = -offset;

			InvalidateMeasure();
		}

		public void SetVerticalOffset(double offset)
		{
			if (offset < 0 || _viewPort.Height >= _extent.Height)
				offset = 0;
			else
			{
				if (offset + _viewPort.Height > _extent.Height)
					offset = _extent.Height - _viewPort.Height;
			}

			_offset.Y = offset;
			if (_scrollOwner != null)
				_scrollOwner.InvalidateScrollInfo();

			_translateTransform.Y = -offset;

			InvalidateMeasure();
		}

		public double VerticalOffset
		{
			get { return _offset.Y; }
		}

		public double ViewportHeight
		{
			get { return _viewPort.Height; }
		}

		public double ViewportWidth
		{
			get { return _viewPort.Width; }
		}

		#endregion
	}
}
