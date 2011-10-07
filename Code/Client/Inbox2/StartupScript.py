from System import *
from System import Environment
from System.Windows import *
from System.Windows.Media import *
from System.Windows.Media.Animation import *
from System.Windows.Controls import *
from System.Windows.Shapes import *
from Inbox2.Platform.Channels.Entities import *

def BooleanToVisibility(bool):
    return Visibility.Visible if bool else Visibility.Collapsed	

def CountToVisibility(count, min):
    return Visibility.Collapsed if count > min else Visibility.Visible

def NotBooleanToVisibility(bool):
    return Visibility.Visible if not bool else Visibility.Collapsed

def EmptyStringToVisibility(str):	
	return Visibility.Collapsed if String.IsNullOrEmpty(str) else Visibility.Visible

def NotEmptyStringToVisibility(str):
	return Visibility.Collapsed if not String.IsNullOrEmpty(str) else Visibility.Visible

def EmptyStringToSwitch(str, obj1, obj2):
	return obj1 if String.IsNullOrEmpty(str) else obj2
	
def BooleanToVisible(bool):
    return Visibility.Visible if bool else Visibility.Hidden

def BooleanToHidden(bool):
    return Visibility.Visible if bool else Visibility.Hidden

def BooleanToHidden(bool):
    return Visibility.Visible if not bool else Visibility.Hidden

def NullToVisibility(obj):
	return Visibility.Visible if obj is None else Visibility.Collapsed

def NullAndBoolToVisibility(obj, bool):
	return Visibility.Visible if obj is None and bool else Visibility.Collapsed

def NotNullToVisibility(obj):
	return Visibility.Visible if obj is not None else Visibility.Collapsed
	
def NotNullAndBoolToVisibility(obj, bool):
	return Visibility.Visible if obj is not None and bool else Visibility.Collapsed

def FormatPercent(percent):
	return String.Format("{0}%", percent)

def BooleanMargin(bool, double):
	return Thickness(double) if bool else Thickness(0)

def BooleanToLeftMargin(bool, double):
	return Thickness(double, 0, 0, 0) if bool else Thickness(0)

def BooleanToOpacity(bool, double):
	return double if bool else 1

def BooleanToColorSelection(bool, color1, color2):
	return color1 if bool else color2

def BooleanToBold(bool):
	return FontWeights.Bold if bool else FontWeights.Normal

def IsValidEmail(str):
	return SourceAddress.IsValidEmail(str)

def AtReply(status):
	return String.Format("@{0} ", status.From.Address) if status.SourceChannel.DisplayName == "Twitter" else String.Empty

def StringConcat(str1, str2):
	return String.Concat(str1, str2)