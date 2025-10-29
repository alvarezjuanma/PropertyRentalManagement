using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyRentalManagement.Data;
using PropertyRentalManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyRentalManagement.Controllers
{
    public class MessagesController : Controller
    {
        private readonly PRMDbContext _context;

        public MessagesController(PRMDbContext context)
        {
            _context = context;
        }

        // este es para el manager solito
        //public async Task<IActionResult> Index()
        //{          
        //    var currentUserEmail = User.Identity.Name;
        //    var messages = await _context.Messages
        //        .Where(m => m.Recipient == currentUserEmail)
        //        .OrderByDescending(m => m.SentDate)
        //        .ToListAsync();

        //    ViewBag.Tenants = await _context.Tenants
        //       .Select(t => new { t.TenantId, t.Name })
        //       .ToListAsync();

        //    // Recuperar ManagerId desde la sesión
        //    var managerIdString = HttpContext.Session.GetString("ManagerId");
        //    if (string.IsNullOrEmpty(managerIdString) || !int.TryParse(managerIdString, out int managerId))
        //    {
        //        TempData["ErrorMessage"] = "Manager session expired. Please log in again.";
        //        return RedirectToAction("Login", "Account");
        //    }

        //    // Mensajes recibidos
        //    var receivedMessages = await _context.Messages
        //        //.Where(m => m.ManagerId == managerId && m.Recipient == User.Identity.Name)
        //        .Where(m => m.ManagerId == managerId)
        //        .OrderByDescending(m => m.SentDate)
        //        .ToListAsync();

        //    // Mensajes enviados
        //    var sentMessages = await _context.Messages
        //        .Where(m => m.ManagerId == managerId)
        //        .OrderByDescending(m => m.SentDate)
        //        .ToListAsync();

        //    ViewBag.ReceivedMessages = receivedMessages;
        //    ViewBag.SentMessages = sentMessages;
        //    return View(messages);
        //}

        // GET: Messages/GetMessageById/5

        public async Task<IActionResult> Index()
        {
            var currentUserEmail = User.Identity.Name;

            // Verifica si el usuario es un manager o un tenant según la sesión
            var tenantIdString = HttpContext.Session.GetString("TenantId");
            var managerIdString = HttpContext.Session.GetString("ManagerId");

            if (string.IsNullOrEmpty(tenantIdString) && string.IsNullOrEmpty(managerIdString))
            {
                TempData["ErrorMessage"] = "Session expired. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            List<Message> receivedMessages;
            List<Message> sentMessages;

            if (!string.IsNullOrEmpty(managerIdString) && int.TryParse(managerIdString, out int managerId))
            {
               
                receivedMessages = await _context.Messages
                    .Where(m => m.ManagerId == managerId && m.Recipient == currentUserEmail)
                    .OrderByDescending(m => m.SentDate)
                    .ToListAsync();

                sentMessages = await _context.Messages
                    .Where(m => m.ManagerId == managerId)
                    .OrderByDescending(m => m.SentDate)
                    .ToListAsync();
            }
            else if (!string.IsNullOrEmpty(tenantIdString) && int.TryParse(tenantIdString, out int tenantId))
            {
               
                receivedMessages = await _context.Messages
                    .Where(m => m.TenantId == tenantId && m.Recipient == currentUserEmail)
                    .OrderByDescending(m => m.SentDate)
                    .ToListAsync();

                sentMessages = await _context.Messages
                    .Where(m => m.TenantId == tenantId)
                    .OrderByDescending(m => m.SentDate)
                    .ToListAsync();
            }
            else
            {
                TempData["ErrorMessage"] = "Session expired. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            
            ViewBag.Tenants = await _context.Tenants
                .Select(t => new { t.TenantId, t.Name })
                .ToListAsync();

            ViewBag.ReceivedMessages = receivedMessages;
            ViewBag.SentMessages = sentMessages;

            return View(receivedMessages);
        }






        [HttpGet]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var message = await _context.Messages
                .Include(m => m.Tenant)
                .Include(m => m.Manager)
                .FirstOrDefaultAsync(m => m.MessageId == id);

            if (message == null)
            {
                return NotFound();
            }

            if (!message.IsRead)
            {
                message.IsRead = true;
                _context.SaveChanges();
            }

            return Json(new
            {
                senderName = message.Sender,
                subject = message.Subject,
                sentAt = message.SentDate,
                body = message.Content
            });
        }

        //este es el create para tenant solito
        //public async Task<IActionResult> Create()
        //{


        //    // Obtener la lista de managers para el desplegable
        //    ViewBag.Managers = await _context.PropertyManagers
        //        .Select(m => new { m.ManagerId, m.Name, m.Email })
        //        .ToListAsync();

        //    // Crear una nueva instancia de Message para pasar a la vista
        //    var message = new Message();

        //    // Comprobar si hay un TenantId en la sesión para personalizar el mensaje
        //    var tenantIdString = HttpContext.Session.GetString("TenantId");
        //    if (string.IsNullOrEmpty(tenantIdString) || !int.TryParse(tenantIdString, out int tenantId))
        //    {
        //        ModelState.AddModelError("Sender", "Tenant session expired or not set. Please log in again.");
        //    }
        //    else
        //    {
        //        // Verificar que el Tenant existe en la base de datos
        //        var tenant = await _context.Tenants.FindAsync(tenantId);
        //        if (tenant != null)
        //        {
        //            message.Sender = tenant.Email;
        //            message.TenantId = tenantId;
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("Sender", "Invalid Tenant.");
        //        }
        //    }

        //    return View(message);
        //}

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("ManagerId")))
            {
                
                ViewBag.Tenants = await _context.Tenants
                    .Select(t => new { t.TenantId, t.Name, t.Email })
                    .ToListAsync();
                
                ViewBag.IsManagerLoggedIn = true;
            }
            else if (!string.IsNullOrEmpty(HttpContext.Session.GetString("TenantId")))
            {
                
                ViewBag.Managers = await _context.PropertyManagers
                    .Select(m => new { m.ManagerId, m.Name, m.Email })
                    .ToListAsync();
               
                ViewBag.IsManagerLoggedIn = false;
            }
            else
            {
                
                TempData["ErrorMessage"] = "Session expired. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Tenants ??= new List<object>(); 
            ViewBag.Managers ??= new List<object>();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenantId,ManagerId,Subject,Content")] Message message)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("ManagerId")))
            {
                return await SendByManager(message);
            }
            else if (!string.IsNullOrEmpty(HttpContext.Session.GetString("TenantId")))
            {
                return await SendByTenant(message);
            }
            else
            {
                TempData["ErrorMessage"] = "Session expired. Please log in again.";
                return RedirectToAction("Login", "Account");
            }
        }



        //public async Task<IActionResult> Create([Bind("ManagerId,Subject,Content")] Message message)
        private async Task<IActionResult> SendByTenant(Message message)
        {
            
            var tenantIdString = HttpContext.Session.GetString("TenantId");
            if (string.IsNullOrEmpty(tenantIdString) || !int.TryParse(tenantIdString, out int tenantId))
            {
                ModelState.AddModelError("Sender", "Tenant session expired or not set. Please log in again.");
                ViewBag.Managers = await _context.PropertyManagers
                    .Select(m => new { m.ManagerId, m.Name, m.Email })
                    .ToListAsync();
                return View(message);
            }

            
            var tenant = await _context.Tenants.FindAsync(tenantId);
            if (tenant == null)
            {
                ModelState.AddModelError("Sender", "Invalid Tenant.");
                ViewBag.Managers = await _context.PropertyManagers
                    .Select(m => new { m.ManagerId, m.Name, m.Email })
                    .ToListAsync();
                return View(message);
            }

            
            message.Sender = tenant.Email; 
            message.TenantId = tenantId; 
            message.SentDate = DateTime.Now; 

           
            var manager = await _context.PropertyManagers.FindAsync(message.ManagerId);
            if (manager == null)
            {
                ModelState.AddModelError("Recipient", "Invalid manager selected.");
                ViewBag.Managers = await _context.PropertyManagers
                    .Select(m => new { m.ManagerId, m.Name, m.Email })
                    .ToListAsync();
                return View(message);
            }

            message.Recipient = manager.Email;

            
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Message sent!";
            return RedirectToAction(nameof(Index));
        }



        //public async Task<IActionResult> Create([Bind("TenantId,Subject,Content")] Message message)
        private async Task<IActionResult> SendByManager(Message message)
        {

            var managerIdString = HttpContext.Session.GetString("ManagerId");
            if (string.IsNullOrEmpty(managerIdString) || !int.TryParse(managerIdString, out int managerId))
            {
                ModelState.AddModelError("Sender", "Manager session expired or not set. Please log in again.");
                ViewBag.Tenants = await _context.Tenants
                    .Select(t => new { t.TenantId, t.Name, t.Email })
                    .ToListAsync();
                return View(message);
            }


            var manager = await _context.PropertyManagers.FindAsync(managerId);
            if (manager == null)
            {
                ModelState.AddModelError("Sender", "Invalid Manager.");
                ViewBag.Tenants = await _context.Tenants
                    .Select(t => new { t.TenantId, t.Name, t.Email })
                    .ToListAsync();
                return View(message);
            }


            message.Sender = manager.Email;
            message.ManagerId = managerId;
            message.SentDate = DateTime.Now;


            var tenant = await _context.Tenants.FindAsync(message.TenantId);
            if (tenant == null)
            {
                ModelState.AddModelError("Recipient", "Invalid tenant selected.");
                ViewBag.Tenants = await _context.Tenants
                    .Select(t => new { t.TenantId, t.Name, t.Email })
                    .ToListAsync();
                return View(message);
            }

            message.Recipient = tenant.Email;


            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewBag.Tenants = await _context.Tenants
                .Select(t => new { t.TenantId, t.Name, t.Email })
                .ToListAsync();
            TempData["SuccessMessage"] = "Message sent!";
            return RedirectToAction(nameof(Index));
            return View(message);
        }




        public async Task<IActionResult> Reply(int id)
        {
            var originalMessage = await _context.Messages.FindAsync(id);
            if (originalMessage == null)
            {
                return NotFound();
            }

            var replyMessage = new Message
            {
                Recipient = originalMessage.Sender,
                Subject = "Re: " + originalMessage.Subject,
                Sender = User.Identity.Name, // Ajusta según cómo manejas la autenticación
                SentDate = DateTime.Now
            };

            return View(replyMessage);
        }

        // POST: Messages/Reply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply([Bind("Sender,Recipient,Subject,Content,TenantId,ManagerId")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.SentDate = DateTime.Now;
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }
    }
}
