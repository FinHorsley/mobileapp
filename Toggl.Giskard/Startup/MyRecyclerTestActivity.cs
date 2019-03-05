using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using System;
using System.Collections.Immutable;
using Toggl.Foundation.MvvmCross.Collections;
using Toggl.Giskard.Adapters;
using System.Reactive.Linq;
using System.Linq;
using Android.Widget;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Toggl.Giskard.Activities
{
    using TimeEntryGroup = CollectionSection<NGroup, NTimeEntry>;
    using DaySection = CollectionSection<NSummaryDayVM, CollectionSection<NGroup, NTimeEntry>>;

    [Activity(
        MainLauncher = true,
        Theme = "@style/AppTheme",
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize
    )]
    public class WheelPlaygroundActivity : AppCompatActivity
    {
        private RecyclerView recycler;
        private Button resetButton;
        private Random rand = new Random();
        private UniversalAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MyRecyclerTest);

            adapter = new UniversalAdapter(new DaySectionAdapterConfiguration());

            var data = createData();

            adapter.UpdateItems(data);

            recycler = FindViewById<RecyclerView>(Resource.Id.BeautifulTree);
            recycler.SetLayoutManager(new LinearLayoutManager(this));
            recycler.SetAdapter(adapter);

            resetButton = FindViewById<Button>(Resource.Id.BeautifulResetButton);
            resetButton.Click += updateData;
        }

        private void updateData(object sender, EventArgs e)
        {
            var sw = Stopwatch.StartNew();
            var data = createData();
            sw.Stop();
            System.Diagnostics.Debug.WriteLine($"CREATE DATA :: {sw.ElapsedTicks / (double)Stopwatch.Frequency}");

            adapter.UpdateItems(data);
        }

        private ImmutableList<DaySection> createData()
        {
            var now = DateTimeOffset.Now - TimeSpan.FromDays(1);

            var daysCount = rand.Next(6, 14);
            daysCount = 60;
            var data = Enumerable.Range(0, daysCount)
                .Select(i => now - TimeSpan.FromDays(i))
                .Select(day => createDayGroup(day))
                .ToImmutableList();

            return data;
        }

        private DaySection createDayGroup(DateTimeOffset day)
        {
            var dayID = $"{day:yyyy-MM-dd}";
            var header = new NSummaryDayVM(day);

            var count = rand.Next(8, 12);
            count = 20;
            var entryGroups = Enumerable.Range(0, count)
               .Select(i => createTimeEntryGroup($"{dayID} {i + 1}."))
               .ToImmutableList();

            return new DaySection(header, entryGroups);
        }

        private TimeEntryGroup createTimeEntryGroup(string groupID)
        {
            var header = new NGroup(groupID);

            var count = rand.Next(3, 7);
            count = 10;
            var timeEntries = Enumerable.Range(0, count)
                .Select(i => new NTimeEntry($"{groupID}{i + 1}."))
                .ToImmutableList();

            return new TimeEntryGroup(header, timeEntries);
        }
    }
}
