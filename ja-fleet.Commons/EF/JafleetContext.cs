using Microsoft.EntityFrameworkCore;

namespace jafleet.Commons.EF
{
    public partial class JafleetContext : DbContext
    {
        public virtual DbSet<Aircraft> Aircrafts { get; set; }
        public virtual DbSet<AircraftView> AircraftViews { get; set; }
        public virtual DbSet<Airline> Airlines { get; set; }
        public virtual DbSet<AirlineGroup> AirlineGroups { get; set; }
        public virtual DbSet<Code> Codes { get; set; }
        public virtual DbSet<Maker> Makers { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<TypeDetail> TypeDetails { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<LineUser> LineUsers { get; set; }
        public virtual DbSet<SearchCondition> SearchConditions { get; set; }
        public virtual DbSet<DailyStatistics> DailyStatistics { get; set; }
        public virtual DbSet<TypeDetailView> TypeDetailViews { get; set; }
        public virtual DbSet<Message> Messagess { get; set; }
        public virtual DbSet<AircraftHistory> AircraftHistories { get; set; }
        public virtual DbSet<AircraftHistoryView> AircraftHistoryViews { get; set; }
        public virtual DbSet<AircraftPhoto> AircraftPhotos { get; set; }
        public virtual DbSet<SeatConfiguration> SeatConfigrations { get; set; }
        public virtual DbSet<WorkingStatus> WorkingStatuses { get; set; }
        public virtual DbSet<AccessLog> AccessLogs { get; set; }
        public virtual DbSet<AppInfo> AppInfos { get; set; }
        public virtual DbSet<SchedulerDef> SchedulerDefs { get; set; }
        public virtual DbSet<JcabImportSession> JcabImportSessions { get; set; }

        public JafleetContext(DbContextOptions<JafleetContext> options) : base(options) => AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

    }
}