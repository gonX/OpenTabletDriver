using System.Collections.Generic;
using Eto.Drawing;
using Eto.Forms;

namespace OpenTabletDriver.UX.Windows
{
    public class AboutWindow : DesktopForm
    {
        const int LARGE_FONTSIZE = 14;
        const int FONTSIZE = LARGE_FONTSIZE - 4;
        const int SPACING = 10;

        private static readonly Dictionary<string, IReadOnlyList<string>> Contributors = new()
        {
            { "Developers", ["InfinityGhost", "X9VoiD", "gonX", "jamesbt365", "Kuuube", "AkiSakurai"] },
            { "Designers", ["InfinityGhost"] },
            { "Documenters", ["InfinityGhost", "gonX", "jamesbt365", "Kuuube"] }
        };

        public AboutWindow()
            : base(Application.Instance.MainForm)
        {
            Title = "About OpenTabletDriver";

            var aboutTabContent = new StackLayout
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = SPACING,
                Spacing = SPACING / 2,
                Items =
                {
                    new ImageView
                    {
                        Image = new Bitmap(App.Logo.WithSize(256, 256)),
                    },
                    new Label
                    {
                        Text = "OpenTabletDriver",
                        VerticalAlignment = VerticalAlignment.Center,
                        Font = SystemFonts.Bold(FONTSIZE),
                    },
                    new Label
                    {
                        Text = $"v{App.Version}",
                        VerticalAlignment = VerticalAlignment.Center,
                    },
                    new Label
                    {
                        Text = "Open source, cross-platform tablet configurator",
                        VerticalAlignment = VerticalAlignment.Center,
                    },
                    new LinkButton
                    {
                        Text = "OpenTabletDriver Github Repository",
                        Command = new Command((s, e) => Application.Instance.Open(App.Website.ToString())),
                    }
                }
            };

            var creditsTabContent = GenerateAboutPageStackLayout();
            creditsTabContent.Items.Add(GenerateHeaderLabel("Credits"));
            AppendContributors(ref creditsTabContent);

            var licenseTabContent = GenerateAboutPageStackLayout();
            licenseTabContent.Items.Add(GenerateHeaderLabel("License"));
            licenseTabContent.Items.Add(new StackLayoutItem {
                Expand = true,
                Control = new TextArea
                {
                    ReadOnly = true,
                    Text = App.License,
                }
            });

            var tabControl = new TabControl();
            tabControl.Pages.Add(new TabPage(aboutTabContent) { Text = "About" });
            tabControl.Pages.Add(new TabPage(creditsTabContent) { Text = "Credits" });
            tabControl.Pages.Add(new TabPage(licenseTabContent) { Text = "License" });

            this.Content = tabControl;
        }

        private static void AppendContributors(ref StackLayout layout)
        {
            foreach (var contributorGroup in Contributors)
            {
                var sl = GenerateAboutPageStackLayout();

                sl.Items.Add(new Label
                {
                    Font = SystemFonts.Bold(LARGE_FONTSIZE),
                    Text = contributorGroup.Key,
                    TextAlignment = TextAlignment.Center,
                });

                foreach (string contributor in contributorGroup.Value)
                    sl.Items.Add(new Label
                    {
                        TextAlignment = TextAlignment.Center,
                        Text = contributor
                    });

                layout.Items.Add(sl);
            }
        }

        private static Label GenerateHeaderLabel(string title) => new()
            {
                Text = $"OpenTabletDriver v{App.Version} {title}",
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Font = SystemFonts.Bold(FONTSIZE),
                Width = 250,
            };

        private static StackLayout GenerateAboutPageStackLayout() => new()
            {
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Padding = SPACING,
                Spacing = SPACING / 2,
                Width = 500,
            };
    }
}
