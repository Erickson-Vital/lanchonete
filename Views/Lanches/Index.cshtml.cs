using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lanchonete.Views.Lanches
{
    [Authorize(Roles = "admin")]
    public class TesteModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
