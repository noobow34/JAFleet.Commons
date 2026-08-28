using System.Text;

namespace JAFleet.Commons
{
    public static class SeatConfigUtil
    {
        public static string GenerateSeatConfigString(int? F, int? C, int? PY, int? P, int? J, int? Y)
        {
            int sum = (F ?? 0) + (C ?? 0) + (PY ?? 0) + (P ?? 0) + (J ?? 0) + (Y ?? 0);

            var seatConfig = new StringBuilder();
            int kindCount = 0;
            bool notFirst = false;
            if (F.HasValue)
            {
                seatConfig.Append("F");
                seatConfig.Append(F.Value);
                notFirst = true;
                kindCount++;
            }
            if (C.HasValue)
            {
                seatConfig.Append(notFirst ? "," : string.Empty);
                seatConfig.Append("C");
                seatConfig.Append(C.Value);
                notFirst = true;
                kindCount++;
            }
            if (PY.HasValue)
            {
                seatConfig.Append(notFirst ? "," : string.Empty);
                seatConfig.Append("PY");
                seatConfig.Append(PY.Value);
                notFirst = true;
                kindCount++;
            }
            if (P.HasValue)
            {
                seatConfig.Append(notFirst ? "," : string.Empty);
                seatConfig.Append("P");
                seatConfig.Append(P.Value);
                notFirst = true;
                kindCount++;
            }
            if (J.HasValue)
            {
                seatConfig.Append(notFirst ? "," : string.Empty);
                seatConfig.Append("J");
                seatConfig.Append(J.Value);
                notFirst = true;
                kindCount++;
            }
            if (Y.HasValue)
            {
                seatConfig.Append(notFirst ? "," : string.Empty);
                seatConfig.Append("Y");
                seatConfig.Append(Y.Value);
                kindCount++;
            }

            if (kindCount > 1)
            {
                seatConfig.Append($" ({sum.ToString()})");
            }

            return seatConfig.ToString();
        }
        public static string GenerateSeatConfigLongString(int? F, int? C, int? PY, int? P, int? J, int? Y, string airline, string type)
        {
            return GenerateSeatConfigString(F, C, PY, P, J, Y) + $" ({airline}-{type})";
        }
    }

}
