using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopApp.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IBasketService basketService;
        IOrderService orderService;
        IRepository<Customer> customerRepository;
        public BasketController(IBasketService _basketService,IOrderService _orderService, IRepository<Customer> _customerRepository)
        {
            basketService = _basketService;
            orderService = _orderService;
            customerRepository = _customerRepository;
        }
        // GET: Basket
        public ActionResult Index()
        {
            var model = basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }
        public ActionResult AddToBasket(string Id)
        {
            basketService.AddToBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }
        public ActionResult RemoveFromBasket(string Id)
        {
            basketService.RemoveFromBasket(this.HttpContext, Id);
            return RedirectToAction("Index");

        }
        public PartialViewResult BasketSummary()
        {
            var basketSummary = basketService.GetBasketSummary(this.HttpContext);
            return PartialView("BasketSummary",basketSummary);
        }
        [Authorize]
        public ActionResult CheckOut()
        {
            Customer customer = customerRepository.Collection().FirstOrDefault(c => c.Email == User.Identity.Name);
            if (customer!=null)
            {
                Order order = new Order()
                {
                    FirstName = customer.FirstName,
                    SurName = customer.LastName,
                    Email = customer.Email,
                    Street = customer.Street,
                    State = customer.State,
                    City = customer.City,
                    ZipCode = customer.ZipCode
                };
                return View(order);
                
            }
            else
            {
                return RedirectToAction("Error");
            }
         
        }
        [HttpPost]
        [Authorize]
        public ActionResult CheckOut(Order order)
        {
            var basketItems = basketService.GetBasketItems(this.HttpContext);
            order.OrderStatus = "Order Created";
            order.Email = User.Identity.Name;

            //Process Payment

            order.OrderStatus = "Payment Processed";
            orderService.CreateOrder(order, basketItems);
            basketService.ClearBasket(this.HttpContext);

            return RedirectToAction("ThankYou",new {OrderId=order.Id });
        }
        public ActionResult ThankYou(string OrderId)
        {
            ViewBag.OrderId = OrderId;
            return View();
        }
    }
}