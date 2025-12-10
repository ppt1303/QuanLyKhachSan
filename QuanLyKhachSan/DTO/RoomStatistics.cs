namespace QuanLyKhachSan.DTO
{
    public class RoomStatistics
    {
        public int TongSo { get; set; }
        public int Trong { get; set; }
        public int DangO { get; set; }
        public int DatTruoc { get; set; }
        public int Ban { get; set; }
        public int BaoTri { get; set; }

        public RoomStatistics()
        {
            TongSo = 0; Trong = 0; DangO = 0; DatTruoc = 0; Ban = 0; BaoTri = 0;
        }
    }
}