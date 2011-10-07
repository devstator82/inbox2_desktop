using System;
using System.Windows.Controls;
using Inbox2.Platform.Channels.Configuration;

namespace Inbox2.UI.Controls.Setup
{
    /// <summary>
    /// Interaction logic for ChannelAddControl.xaml
    /// </summary>
    public partial class ChannelAddControl : UserControl
    {
        /// <summary>
        /// Gets or sets the channel configuration.
        /// </summary>
        /// <value>The channel configuration.</value>
        public ChannelConfiguration ChannelConfiguration { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelAddControl"/> class.
        /// </summary>
        public ChannelAddControl()
        {
            InitializeComponent();
            
            DataContext = this;
        }
    }
}
