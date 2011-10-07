using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MsHtmHstInterop;

namespace Inbox2.Framework.UI
{
	public class DocHostUIHandler : IDocHostUIHandler
	{
		private ObjectForScriptingHelper helper = new ObjectForScriptingHelper();

		public void ShowContextMenu(uint dwID, ref tagPOINT ppt, object pcmdtReserved, object pdispReserved)
		{
			//throw new COMException("", 1); // HRESULT = S_FALSE: MSHTML will show its menu
		}

		public void GetHostInfo(ref _DOCHOSTUIINFO pInfo)
		{
			pInfo.dwFlags |= (uint)tagDOCHOSTUIFLAG.DOCHOSTUIFLAG_SCROLL_NO | (uint)tagDOCHOSTUIFLAG.DOCHOSTUIFLAG_NO3DBORDER;
		}

		public void ShowUI(uint dwID, IOleInPlaceActiveObject pActiveObject, IOleCommandTarget pCommandTarget, IOleInPlaceFrame pFrame, IOleInPlaceUIWindow pDoc)
		{
		}

		public void HideUI()
		{
		}

		public void UpdateUI()
		{
		}

		public void EnableModeless(int fEnable)
		{
		}

		public void OnDocWindowActivate(int fActivate)
		{
		}

		public void OnFrameWindowActivate(int fActivate)
		{
		}

		public void ResizeBorder(ref tagRECT prcBorder, IOleInPlaceUIWindow pUIWindow, int fRameWindow)
		{
		}

		public void TranslateAccelerator(ref tagMSG lpmsg, ref Guid pguidCmdGroup, uint nCmdID)
		{
			throw new COMException("", 1); // returns HRESULT = S_FALSE
		}

		public void GetOptionKeyPath(out string pchKey, uint dw)
		{
			pchKey = null;
		}

		public void GetDropTarget(IDropTarget pDropTarget, out IDropTarget ppDropTarget)
		{
			ppDropTarget = null;
		}

		public void GetExternal(out object ppDispatch)
		{
			ppDispatch = helper;
		}

		public void TranslateUrl(uint dwTranslate, ref ushort pchURLIn, IntPtr ppchURLOut)
		{
		}

		public void FilterDataObject(IDataObject pDO, out IDataObject ppDORet)
		{
			ppDORet = null;
		}
	}
}
