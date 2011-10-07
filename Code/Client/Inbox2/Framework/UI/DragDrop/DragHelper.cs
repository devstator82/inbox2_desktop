using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Shapes;
using Inbox2.Framework.Interop;

namespace Inbox2.Framework.UI.DragDrop
{
	public class DragHelper : DependencyObject
	{
		private IDataDropObjectProvider _callback;
		private UIElement _dragSource;
		private UIElement _dragScope;
		private double _opacity = 0.3;
		private bool _isDragging;
		private Point _startPoint;
		private DragAdorner _adorner;
		private AdornerLayer _layer;
		private Window _dragdropWindow;

		public DragHelper(UIElement source, IDataDropObjectProvider callback, UIElement dragScope)
		{
			this._dragScope = dragScope;
			this._callback = callback;
			this._dragSource = source;

			WireEvents(source, dragScope);
		}

		private void WireEvents(UIElement uie, Visual scope)
		{

			if (uie != null)
			{

				uie.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(DragSource_PreviewMouseLeftButtonDown);
				uie.PreviewMouseMove += new System.Windows.Input.MouseEventHandler(DragSource_PreviewMouseMove);

#if DEBUG
				uie.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(DragSource_PreviewMouseLeftButtonUp);
#endif
			}

		}

		void DragSource_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			_startPoint = e.GetPosition(DragScope);
		}

#if DEBUG
		void DragSource_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
		}
#endif

		void DragSource_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{

			if (e.LeftButton == MouseButtonState.Pressed && !IsDragging)
			{
				Point position = e.GetPosition((IInputElement)DragScope);
				if (Math.Abs(position.X - _startPoint.X) > 100 ||
					Math.Abs(position.Y - _startPoint.Y) > 100)
				{
					StartDrag(e);
				}
			}
		}

		#region "members and DPs"

		protected UIElement DragScope
		{
			get
			{
				return _dragScope;
			}
		}

		public double Opacity
		{
			get
			{
				return _opacity;
			}
		}

		protected bool IsDragging
		{
			get
			{
				return _isDragging;
			}
			set
			{
				_isDragging = value;
			}
		}
		protected DragDropEffects _allowedEffects = DragDropEffects.Copy | DragDropEffects.Move;
		public DragDropEffects AllowedEffects
		{
			get
			{
				return _allowedEffects;
			}
			set
			{
				_allowedEffects = value;
			}
		}

		protected bool AllowsLink
		{
			get
			{
				return ((this.AllowedEffects & DragDropEffects.Link) != 0);
			}
		}

		protected bool AllowsMove
		{
			get
			{
				return ((this.AllowedEffects & DragDropEffects.Move) != 0);
			}
		}

		protected bool AllowsCopy
		{
			get
			{
				return ((this.AllowedEffects & DragDropEffects.Copy) != 0);
			}
		}

		protected bool _mouseLeftScope = false;


		#endregion

		UIElement GetDragElementOnHitTest(UIElement src, MouseEventArgs args)
		{
			HitTestResult hr = VisualTreeHelper.HitTest(src, args.GetPosition((IInputElement)src));
			return hr.VisualHit as UIElement;

		}

		void StartDrag(MouseEventArgs args)
		{

			IDataObject data = null;
			UIElement dragelement = null;


			// ADD THE DATA 
			if (_callback != null)
			{
				DragDataWrapper dw = new DragDataWrapper();

				data = new DataObject(typeof(DragDataWrapper).ToString(), dw);

				if ((_callback.SupportedActions & DragDropProviderActions.MultiFormatData) != 0)
					_callback.AppendData(ref data, args);

				if ((_callback.SupportedActions & DragDropProviderActions.Data) != 0)
					dw.Data = _callback.GetData();

				if ((_callback.SupportedActions & DragDropProviderActions.Visual) != 0)
					dragelement = _callback.GetVisual(args);
				else
					dragelement = args.OriginalSource as UIElement;

				dw.Source = _dragSource;
				dw.Shim = _callback;
			}
			else
			{

				dragelement = args.OriginalSource as UIElement;
				data = new DataObject(typeof(UIElement).ToString(), dragelement);
			}


			// mwa changed from:
			// if (dragelement == null || data == null || dragelement == this._dragSource)
			if (dragelement == null || data == null)
				return;

			DragEventHandler dragOver = null;
			DragEventHandler dragLeave = null;
			QueryContinueDragEventHandler queryContinue = null;
			GiveFeedbackEventHandler giveFeedback = null;


			DragDropEffects effects = GetDragDropEffects();
			DragDropEffects resultEffects;

			// Inprocess Drag  ... 
			if (DragScope != null)
			{
				bool previousAllowDrop = DragScope.AllowDrop;
				_adorner = new DragAdorner(DragScope, (UIElement)dragelement, true, Opacity);
				_layer = AdornerLayer.GetAdornerLayer(DragScope as Visual);
				_layer.Add(_adorner);

				if (DragScope != _dragSource)
				{
					DragScope.AllowDrop = true;
					System.Windows.DragDrop.AddPreviewDragOverHandler((DependencyObject)DragScope, dragOver = new DragEventHandler(DragScope_DragOver));
					System.Windows.DragDrop.AddPreviewDragLeaveHandler(DragScope, dragLeave = new DragEventHandler(DragScope_DragLeave));
					System.Windows.DragDrop.AddPreviewQueryContinueDragHandler(_dragSource, queryContinue = new QueryContinueDragEventHandler(onQueryContinueDrag));
				}

				try
				{
					IsDragging = true;
					this._mouseLeftScope = false;
					resultEffects = System.Windows.DragDrop.DoDragDrop(_dragSource, data, effects);
					DragFinished(resultEffects);
				}
				catch
				{
				}

				if (DragScope != _dragSource)
				{
					DragScope.AllowDrop = previousAllowDrop;
					System.Windows.DragDrop.RemovePreviewDragOverHandler(DragScope, dragOver);
					System.Windows.DragDrop.RemovePreviewDragLeaveHandler(DragScope, dragLeave);
					System.Windows.DragDrop.RemovePreviewQueryContinueDragHandler(_dragSource, queryContinue);
				}
			}
			else
			{

				System.Windows.DragDrop.AddPreviewQueryContinueDragHandler(_dragSource, queryContinue = new QueryContinueDragEventHandler(onQueryContinueDrag));
				System.Windows.DragDrop.AddGiveFeedbackHandler(_dragSource, giveFeedback = new GiveFeedbackEventHandler(onGiveFeedback));
				IsDragging = true;
				CreateDragDropWindow(dragelement);
				this._dragdropWindow.Show();
				try
				{
					resultEffects = System.Windows.DragDrop.DoDragDrop(_dragSource, data, effects);
				}
				finally { }
				DestroyDragDropWindow();
				System.Windows.DragDrop.RemovePreviewQueryContinueDragHandler(_dragSource, onQueryContinueDrag);
				System.Windows.DragDrop.AddGiveFeedbackHandler(_dragSource, onGiveFeedback);
				IsDragging = false;
				DragFinished(resultEffects);
			}
		}



		DragDropEffects GetDragDropEffects()
		{
			DragDropEffects effects = DragDropEffects.None;
			bool ctrl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
			bool shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

			if (ctrl && shift && this.AllowsLink)
				effects |= DragDropEffects.Link;
			else if (ctrl && this.AllowsCopy)
				effects |= DragDropEffects.Copy;
			else if (this.AllowsMove)
				effects |= DragDropEffects.Move;

			return effects;
		}

		void DragScope_DragLeave(object sender, DragEventArgs args)
		{
			if (args.OriginalSource == this.DragScope)
			{
				this._mouseLeftScope = true;
			}
		}

		void onGiveFeedback(object sender, GiveFeedbackEventArgs args)
		{
			args.UseDefaultCursors = false;
			args.Handled = true;
		}

		void onQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			if (this.DragScope == null)
			{
				UpdateWindowLocation();
			}
			else
			{

				Point p = Mouse.GetPosition(this.DragScope);
				if (_adorner != null)
				{
					_adorner.LeftOffset = p.X /* - _startPoint.X */ ;
					_adorner.TopOffset = p.Y /* - _startPoint.Y */ ;
				}


				if (this._mouseLeftScope)
				{
					e.Action = DragAction.Cancel;
					e.Handled = true;
				}
			}
		}

		private void DragScope_DragOver(object sender, DragEventArgs args)
		{
			if (_adorner != null)
			{
				_adorner.LeftOffset = args.GetPosition(DragScope).X /* - _startPoint.X */ ;
				_adorner.TopOffset = args.GetPosition(DragScope).Y /* - _startPoint.Y */ ;
			}
		}

		protected void DestroyDragDropWindow()
		{
			if (this._dragdropWindow != null)
			{
				this._dragdropWindow.Close();
				this._dragdropWindow = null;
			}
		}

		protected void CreateDragDropWindow(Visual dragElement)
		{
			this._dragdropWindow = new Window();
			_dragdropWindow.WindowStyle = WindowStyle.None;
			_dragdropWindow.AllowsTransparency = true;
			_dragdropWindow.AllowDrop = false;
			_dragdropWindow.Background = null;
			_dragdropWindow.IsHitTestVisible = false;
			_dragdropWindow.SizeToContent = SizeToContent.WidthAndHeight;
			_dragdropWindow.Topmost = true;
			_dragdropWindow.ShowInTaskbar = false;

			_dragdropWindow.SourceInitialized += delegate
             	{
             		PresentationSource windowSource = PresentationSource.FromVisual(this._dragdropWindow);
             		IntPtr handle = ((System.Windows.Interop.HwndSource)windowSource).Handle;

             		Int32 styles = WinApi.GetWindowLong(handle, WinApi.GWL_EXSTYLE);
             		WinApi.SetWindowLong(handle, WinApi.GWL_EXSTYLE, styles | WinApi.WS_EX_LAYERED | WinApi.WS_EX_TRANSPARENT);

             	};

			Rectangle r = new Rectangle();
			r.Width = ((FrameworkElement)dragElement).ActualWidth;
			r.Height = ((FrameworkElement)dragElement).ActualHeight;
			r.Fill = new VisualBrush(dragElement);
			this._dragdropWindow.Content = r;

			// we want QueryContinueDrag notification so we can update the window position
			//DragDrop.AddPreviewQueryContinueDragHandler(source, QueryContinueDrag);

			// put the window in the right place to start
			UpdateWindowLocation();
		}

		protected void UpdateWindowLocation()
		{
			if (this._dragdropWindow != null)
			{
				POINT p;
				if (!WinApi.GetCursorPos(out p))
				{
					return;
				}
				this._dragdropWindow.Left = (double)p.x;
				this._dragdropWindow.Top = (double)p.y;
			}
		}

		protected void DragFinished(DragDropEffects ret)
		{
			System.Windows.Input.Mouse.Capture(null);
			if (IsDragging)
			{
				if (DragScope != null)
				{
					AdornerLayer.GetAdornerLayer(DragScope).Remove(_adorner);
					_adorner = null;
				}
				else
				{
					DestroyDragDropWindow();
				}

			}
			IsDragging = false;
		}
	}
}