﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace StockTraderRI.Controls
{
    /// <summary>
    /// Custom Tab control with animations.
    /// </summary>
    /// <remarks>
    /// This customization of the TabControl was required to create the animations for the transition 
    /// between the tab items.
    /// </remarks>
    public class AnimatedTabControl : TabControl
    {
        public static readonly RoutedEvent SelectionChangingEvent = EventManager.RegisterRoutedEvent(
            "SelectionChanging", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AnimatedTabControl));

        private DispatcherTimer timer;

        public AnimatedTabControl()
        {
            DefaultStyleKey = typeof(AnimatedTabControl);
        }

        public event RoutedEventHandler SelectionChanging
        {
            add { AddHandler(SelectionChangingEvent, value); }
            remove { RemoveHandler(SelectionChangingEvent, value); }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(
                (Action)delegate
                {
                    RaiseSelectionChangingEvent();

                    StopTimer();

                    timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 500) };

                    EventHandler handler = null;
                    handler = (sender, args) =>
                    {
                        StopTimer();
                        base.OnSelectionChanged(e);
                    };
                    timer.Tick += handler;
                    timer.Start();
                });
        }

        // This method raises the Tap event
        private void RaiseSelectionChangingEvent()
        {
            var args = new RoutedEventArgs(SelectionChangingEvent);
            RaiseEvent(args);
        }

        private void StopTimer()
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }
    }
}