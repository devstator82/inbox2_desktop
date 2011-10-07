# example StartupScript.py.  
#
# Add a file called StartupScript.py to your executable project. 
# Then, set the "Copy to Output Directory" to "Copy if newer" in the property pane for StartupScript.py.

from System import *
from System import Environment
from System.Windows import *
from System.Windows.Media import *
from System.Windows.Media.Animation import *
from System.Windows.Controls import *
from System.Windows.Shapes import *

def BooleanToVisibility(bool):
    return Visibility.Visible if bool else Visibility.Collapsed
    
def BooleanToHidden(bool):
    return Visibility.Visible if bool else Visibility.Hidden

def FormatPercent(percent):
	return String.Format("{0}%", percent)
	