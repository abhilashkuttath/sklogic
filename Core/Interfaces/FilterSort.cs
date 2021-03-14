namespace Core.Interfaces
{
    public class FilterSort
    {
        public int? brandId { get; set; }
        public int? typeId { get; set; }
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }


    }
}