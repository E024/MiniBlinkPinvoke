using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniBlinkPinvoke
{
    public enum wkeMouseMessage : uint
    {
        WKE_MSG_MOUSEMOVE = 0x0200,
        WKE_MSG_LBUTTONDOWN = 0x0201,
        WKE_MSG_LBUTTONUP = 0x0202,
        WKE_MSG_LBUTTONDBLCLK = 0x0203,
        WKE_MSG_RBUTTONDOWN = 0x0204,
        WKE_MSG_RBUTTONUP = 0x0205,
        WKE_MSG_RBUTTONDBLCLK = 0x0206,
        WKE_MSG_MBUTTONDOWN = 0x0207,
        WKE_MSG_MBUTTONUP = 0x0208,
        WKE_MSG_MBUTTONDBLCLK = 0x0209,
        WKE_MSG_MOUSEWHEEL = 0x020A,
    }
    public enum wkeMouseFlags
    {
        WKE_LBUTTON = 0x01,
        WKE_RBUTTON = 0x02,
        WKE_SHIFT = 0x04,
        WKE_CONTROL = 0x08,
        WKE_MBUTTON = 0x10,
    }

    public enum wkeJSType
    {
        JSTYPE_NUMBER,
        JSTYPE_STRING,
        JSTYPE_BOOLEAN,
        JSTYPE_OBJECT,
        JSTYPE_FUNCTION,
        JSTYPE_UNDEFINED
    }
    public enum wkeMessageSource
    {
        WKE_MESSAGE_SOURCE_HTML,
        WKE_MESSAGE_SOURCE_XML,
        WKE_MESSAGE_SOURCE_JS,
        WKE_MESSAGE_SOURCE_NETWORK,
        WKE_MESSAGE_SOURCE_CONSOLE_API,
        WKE_MESSAGE_SOURCE_OTHER
    }
    public enum wkeMessageType
    {
        WKE_MESSAGE_TYPE_LOG,
        WKE_MESSAGE_TYPE_DIR,
        WKE_MESSAGE_TYPE_DIR_XML,
        WKE_MESSAGE_TYPE_TRACE,
        WKE_MESSAGE_TYPE_START_GROUP,
        WKE_MESSAGE_TYPE_START_GROUP_COLLAPSED,
        WKE_MESSAGE_TYPE_END_GROUP,
        WKE_MESSAGE_TYPE_ASSERT
    }
    public enum wkeMessageLevel
    {
        WKE_MESSAGE_LEVEL_TIP,
        WKE_MESSAGE_LEVEL_LOG,
        WKE_MESSAGE_LEVEL_WARNING,
        WKE_MESSAGE_LEVEL_ERROR,
        WKE_MESSAGE_LEVEL_DEBUG
    }
    public enum wkeConsoleLevel {
        wkeLevelDebug = 4,
        wkeLevelLog = 1,
        wkeLevelInfo = 5,
        wkeLevelWarning = 2,
        wkeLevelError = 3,
        wkeLevelRevokedError = 6,
        wkeLevelLast = wkeLevelInfo
    }
    public enum wkeNavigationAction
    {
        WKE_NAVIGATION_CONTINUE,
        WKE_NAVIGATION_ABORT,
        WKE_NAVIGATION_DOWNLOAD
    }
    public enum wkeNavigationType
    {
        WKE_NAVIGATION_TYPE_LINKCLICK,
        WKE_NAVIGATION_TYPE_FORMSUBMITTE,
        WKE_NAVIGATION_TYPE_BACKFORWARD,
        WKE_NAVIGATION_TYPE_RELOAD,
        WKE_NAVIGATION_TYPE_FORMRESUBMITT,
        WKE_NAVIGATION_TYPE_OTHER
    }

    public enum WkeCursorInfo
    {
        WkeCursorInfoPointer = 0,
        WkeCursorInfoCross = 1,
        WkeCursorInfoHand = 2,
        WkeCursorInfoIBeam = 3,
        WkeCursorInfoWait = 4,
        WkeCursorInfoHelp = 5,
        WkeCursorInfoEastResize = 6,
        WkeCursorInfoNorthResize = 7,
        WkeCursorInfoNorthEastResize = 8,
        WkeCursorInfoNorthWestResize = 9,
        WkeCursorInfoSouthResize = 10,
        WkeCursorInfoSouthEastResize = 11,
        WkeCursorInfoSouthWestResize = 12,
        WkeCursorInfoWestResize = 13,
        WkeCursorInfoNorthSouthResize = 14,
        WkeCursorInfoEastWestResize = 15,
        WkeCursorInfoNorthEastSouthWestResize = 16,
        WkeCursorInfoNorthWestSouthEastResize = 17,
        WkeCursorInfoColumnResize = 18,
        WkeCursorInfoRowResize = 19,
    }
    public enum wkeLoadingResult
    {
        WKE_LOADING_SUCCEEDED,
        WKE_LOADING_FAILED,
        WKE_LOADING_CANCELED
    }

}
