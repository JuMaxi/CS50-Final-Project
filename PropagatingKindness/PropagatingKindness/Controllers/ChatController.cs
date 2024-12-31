using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropagatingKindness.Domain.Interfaces;
using PropagatingKindness.Models;
using PropagatingKindness.Models.Chat;

namespace PropagatingKindness.Controllers;

public class ChatController : Controller
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [Authorize]
    public async Task<IActionResult> MyConversations()
    {
        int userId = GetUserId();
        var chats = await _chatService.GetChats(userId);
        return View(MyConversationsViewModel.FromChats(chats, userId));
    }

    [Authorize]
    public async Task<IActionResult> WithAdvert(int id)
    {
        var result = await _chatService.CreateChat(GetUserId(), id);
        if (result.Success)
            return RedirectToAction("MyConversations");
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> SendMessage([FromRoute]int id, [FromBody]SendMessageRequest request)
    {
        var result = await _chatService.SendMessage(GetUserId(), id, request.Message);
        if (result.Success)
            return Ok(new JsonResult(result.Content));
        return BadRequest();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetMessages([FromRoute] int id)
    {
        var result = await _chatService.GetChat(id);
        if (!result.Success)
            return BadRequest();

        var chat = result.Content;
        var userId = GetUserId();

        if(chat.FromUser.Id != userId && chat.ToUser.Id != userId)
            return BadRequest();

        return new JsonResult(ChatMessagesViewModel.FromChat(chat, userId));
    }

    private int GetUserId()
    {
        return Convert.ToInt32(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }
}
