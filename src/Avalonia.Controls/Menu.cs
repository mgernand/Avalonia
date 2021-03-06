// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia.Controls.Platform;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Controls
{
    /// <summary>
    /// A top-level menu control.
    /// </summary>
    public class Menu : MenuBase, IMainMenu
    {
        private static readonly ITemplate<IPanel> DefaultPanel =
            new FuncTemplate<IPanel>(() => new StackPanel { Orientation = Orientation.Horizontal });

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        public Menu()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class.
        /// </summary>
        /// <param name="interactionHandler">The menu interaction handler.</param>
        public Menu(IMenuInteractionHandler interactionHandler)
            : base(interactionHandler)
        {
        }

        static Menu()
        {
            ItemsPanelProperty.OverrideDefaultValue(typeof(Menu), DefaultPanel);
        }

        /// <inheritdoc/>
        public override void Close()
        {
            if (IsOpen)
            {
                foreach (var i in ((IMenu)this).SubItems)
                {
                    i.Close();
                }

                IsOpen = false;
                SelectedIndex = -1;

                RaiseEvent(new RoutedEventArgs
                {
                    RoutedEvent = MenuClosedEvent,
                    Source = this,
                });
            }
        }

        /// <inheritdoc/>
        public override void Open()
        {
            if (!IsOpen)
            {
                IsOpen = true;

                RaiseEvent(new RoutedEventArgs
                {
                    RoutedEvent = MenuOpenedEvent,
                    Source = this,
                });
            }
        }

        /// <inheritdoc/>
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            var inputRoot = e.Root as IInputRoot;

            if (inputRoot?.AccessKeyHandler != null)
            {
                inputRoot.AccessKeyHandler.MainMenu = this;
            }
        }
    }
}
