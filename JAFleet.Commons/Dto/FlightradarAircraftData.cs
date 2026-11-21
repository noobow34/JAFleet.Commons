namespace JAFleet.Commons.Dto
{
    /// <summary>
    /// Flightradar24 の機体ページ（/data/aircraft/{reg}）から取り出した情報。
    /// ページは Inertia.js の SPA になっており、表示に使う値はすべて
    /// <c>&lt;div id="app" data-page="{...}"&gt;</c> の JSON に入っているのでそれを読む。
    /// </summary>
    public class FlightradarAircraftData
    {
        /// <summary>機体ページとして描画されているか（未知のレジは機種一覧へリダイレクトされる）</summary>
        public bool IsAircraftPage { get; set; }

        /// <summary>JetPhotos の最新写真。1 枚もなければ null</summary>
        public AircraftPhotoResult? Photo { get; set; }

        /// <summary>フライト履歴。新しい順。取得できなければ空</summary>
        public List<FlightradarFlight> Flights { get; set; } = [];
    }

    /// <summary>フライト履歴 1 件。稼働判定に使う項目だけ持つ</summary>
    public class FlightradarFlight
    {
        /// <summary>出発日時（実出発 → 出発予定 → 出発予定時刻の順に採用）</summary>
        public DateTime DepartureDate { get; set; }

        /// <summary>便名。無ければコールサイン</summary>
        public string? FlightNumber { get; set; }

        /// <summary>出発地。`都市名 (IATA)` 形式</summary>
        public string? FromAp { get; set; }

        /// <summary>到着地。`都市名 (IATA)` 形式</summary>
        public string? ToAp { get; set; }

        /// <summary>状況。`Landed 21:27` のような表示用テキスト</summary>
        public string? Status { get; set; }
    }
}
