using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eto.Drawing;
using Eto.Forms;

namespace OpenTabletDriver.UX.Windows
{
    public class AboutWindow : DesktopForm
    {
        private const int LARGE_FONTSIZE = 14;
        private const int FONTSIZE = LARGE_FONTSIZE - 4;
        private const int SPACING = 10;

        private const string JamesText = """
                                         In loving memory of jamesbt365.
                                         One of the biggest contributors to OpenTabletDriver and one of the kindest souls.

                                         James was with the OpenTabletDriver team for about 4 and a half years.

                                         In that time he sent over a quarter million messages on the OpenTabletDriver Discord server, added and improved hundreds of tablet configurations, commented thousands of times on Github issues, and pushed the project as a whole to new heights.

                                         The countless hours he spent helping and improving OpenTabletDriver for not only its users but also the developers will not be forgotten.

                                         His legacy will live on through the tablets he added support for, the features he merged into the driver, and the enormous impact he had on the community.
                                         """;

        // ReSharper disable thrice InconsistentNaming
        // disabled because nameof(field) is used
        private readonly string[] Designers = ["InfinityGhost"];

        private readonly string[] Developers =
            ["InfinityGhost", "X9VoiD", "gonX", "jamesbt365", "Kuuube", "AkiSakurai"];

        private readonly string[] Documenters = ["InfinityGhost", "gonX", "jamesbt365", "Kuuube"];

        private readonly TabPage _memoriamTabPage;

        public AboutWindow()
            : base(Application.Instance.MainForm)
        {
            Title = "About OpenTabletDriver";

            var tabControl = new TabControl();

            tabControl.Pages.Add(GenerateAboutTabContent());
            tabControl.Pages.Add(GenerateCreditsTabContent());
            tabControl.Pages.Add(GenerateLicenseTabContent());
            tabControl.Pages.Add(_memoriamTabPage = GenerateMemoriamTabContent());

            Content = tabControl;

            KeyDown += (_, args) =>
            {
                if (args.Key == Keys.Escape)
                    Close();
                if (args.Key == Keys.J)
                    ShowMemoriamTab();
            };
        }

        private void ShowMemoriamTab()
        {
            Debug.Assert(_memoriamTabPage != null);
            _memoriamTabPage.Visible = true;
        }

        private StackLayoutItem GenerateContributor(IEnumerable<string> contributors, string title)
        {
            return new StackLayoutItem
            {
                Expand = true,
                Control = new StackLayout
                {
                    Items =
                    {
                        new GroupBox
                        {
                            Text = title,
                            Font = SystemFonts.Bold(LARGE_FONTSIZE),
                            Padding = SPACING,
                            Content = new LabelList(contributors, [
                                new CommandLabel
                                {
                                    Text = "jamesbt365",
                                    TextAlignment = TextAlignment.Center,
                                    Command = new Command((_, _) => ShowMemoriamTab())
                                }
                            ])
                        }
                    }
                }
            };
        }

        private static void GenerateStackLayoutItems(ref StackLayout stackLayout, string title, Control control)
        {
            stackLayout.Items.Add(new Label
            {
                Text = title,
                TextAlignment = TextAlignment.Center,
                Font = SystemFonts.Bold(LARGE_FONTSIZE)
            });

            stackLayout.Items.Add(new StackLayoutItem
            {
                Expand = true,
                Control = control
            });
        }

        #region Tab Pages

        private static TabPage GenerateMemoriamTabContent()
        {
            var memoriamTabContent = new StackLayout
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Padding = SPACING,
                Spacing = SPACING / 2
            };

            GenerateStackLayoutItems(ref memoriamTabContent,
                "In Memory of James",
                new Label { Text = JamesText });

            return new TabPage(memoriamTabContent) { Text = "Memoriam", Visible = false };
        }

        private static TabPage GenerateLicenseTabContent()
        {
            var licenseTabContent = new StackLayout
            {
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                Padding = SPACING,
                Spacing = SPACING / 2,
                Width = 500
            };

            var licenseTabContentControl = new TextArea
            {
                ReadOnly = true,
                Text = App.License
            };

            GenerateStackLayoutItems(ref licenseTabContent,
                $"OpenTabletDriver v{App.Version} License",
                licenseTabContentControl);

            return new TabPage(licenseTabContent) { Text = "License" };
        }

        private TabPage GenerateCreditsTabContent()
        {
            var creditsTabContent = new StackLayout
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Padding = SPACING,
                Spacing = SPACING / 2
            };

            var creditsTabControl = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Padding = SPACING,
                Spacing = SPACING,
                Items =
                {
                    GenerateContributor(Developers, nameof(Developers)),
                    GenerateContributor(Designers, nameof(Designers)),
                    GenerateContributor(Documenters, nameof(Documenters))
                }
            };

            GenerateStackLayoutItems(ref creditsTabContent,
                $"OpenTabletDriver v{App.Version} Credits",
                creditsTabControl);

            return new TabPage(creditsTabContent) { Text = "Credits" };
        }

        private TabPage GenerateAboutTabContent()
        {
            var aboutTabContent = new StackLayout
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Padding = SPACING,
                Spacing = SPACING / 2,
                Items =
                {
                    new ImageView
                    {
                        Image = new Bitmap(App.Logo.WithSize(256, 256))
                    },
                    new Label
                    {
                        Text = "OpenTabletDriver",
                        Font = SystemFonts.Bold(LARGE_FONTSIZE)
                    },
                    new Label
                    {
                        Text = $"v{App.Version}"
                    },
                    new Label
                    {
                        Text = "Open source, cross-platform tablet configurator"
                    },
                    new LinkButton
                    {
                        Text = "OpenTabletDriver Github Repository",
                        Command = new Command((_, _) => Application.Instance.Open(App.Website.ToString()))
                    },
                    new CommandLabel
                    {
                        Text = "In memory of jamesbt365",
                        Command = new Command((_, _) => ShowMemoriamTab())
                    }
                }
            };

            return new TabPage(aboutTabContent) { Text = "About" };
        }

        #endregion Tab Pages
    }

    internal class CommandLabel : Label
    {
        public required Command Command;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Command.Execute();
            base.OnMouseDown(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            Font = SystemFonts.Bold();
            Cursor = new Cursor(CursorType.Pointer);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            Font = SystemFonts.Default();
            Cursor = new Cursor(CursorType.Default);
            base.OnMouseLeave(e);
        }
    }

    internal class LabelList : StackLayout
    {
        public LabelList(IEnumerable<string> textArray, CommandLabel[] commandLabels)
        {
            foreach (string text in textArray)
            {
                var commandLabel = Array.Find(commandLabels, x => x.Text == text);

                Items.Add(commandLabel ?? new Label
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    Text = text
                });
            }
        }
    }
}
