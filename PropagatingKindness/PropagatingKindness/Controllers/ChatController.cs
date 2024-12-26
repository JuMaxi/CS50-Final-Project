using Microsoft.AspNetCore.Mvc;

namespace PropagatingKindness.Controllers;

public class ChatController : Controller
{
    public IActionResult MyConversations()
    {
        return View();
    }
}
