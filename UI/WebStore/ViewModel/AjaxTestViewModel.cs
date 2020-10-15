using System;

namespace WebStore.ViewModel
{
    public class AjaxTestViewModel
    {
        public int? Id { get; set; }
        public string Message { get; set; }

        public DateTime ServerTime { get; set; } = DateTime.Now;
    }
}
