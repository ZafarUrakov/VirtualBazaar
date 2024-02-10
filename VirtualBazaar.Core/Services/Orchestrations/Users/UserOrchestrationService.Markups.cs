using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Categories;
using VirtualBazaar.Core.Models.Foundations.Orders;
using VirtualBazaar.Core.Models.Foundations.Products;

namespace VirtualBazaar.Core.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private static ReplyKeyboardMarkup ContactMarkup()
        {
            return new ReplyKeyboardMarkup(new KeyboardButton[][]
            {
               new KeyboardButton[]{ new KeyboardButton("Share contact 📞") 
               { RequestContact = true } },
            })
            {
                ResizeKeyboard = true
            };
        }
        
        private static ReplyKeyboardMarkup LocationMarkup()
        {
            return new ReplyKeyboardMarkup(new KeyboardButton[][]
            {
               new KeyboardButton[]{ new KeyboardButton("Share location 📍") 
               { RequestLocation = true } },
            })
            {
                ResizeKeyboard = true
            };
        }
        
        private static ReplyKeyboardMarkup MenuMarkup()
        {
            var keyboardButtons = new List<KeyboardButton[]>
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Menu 🛍")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Me 👤"),
                    new KeyboardButton("Settings ⚙️"),
                    new KeyboardButton("Review 📝"),
                    new KeyboardButton("Contact us ☎️")
                }
            };

            return new ReplyKeyboardMarkup(keyboardButtons)
            {
                ResizeKeyboard = true
            };
        }
        
        private static ReplyKeyboardMarkup SettingsMarkup()
        {
            var keyboardButtons = new List<KeyboardButton[]>
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Change name 🪄"),
                    new KeyboardButton("Change address 🏡"),
                    new KeyboardButton("Change phone number 📲")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("⬅️ Back")
                }
            };

            return new ReplyKeyboardMarkup(keyboardButtons)
            {
                ResizeKeyboard = true
            };
        }

        private ReplyKeyboardMarkup CategoriesMarkup(IQueryable<Category> categories)
        {
            var buttons = new List<KeyboardButton[]>();
            List<KeyboardButton> rowButtons = new List<KeyboardButton>();
            buttons.Add(new KeyboardButton[] { new KeyboardButton(basketCommand) });

            foreach (var category in categories)
            {
                rowButtons.Add(new KeyboardButton($"{category.Name}"));

                if (rowButtons.Count == 3)
                {
                    buttons.Add(rowButtons.ToArray());
                    rowButtons.Clear();
                }
            }

            if (rowButtons.Count > 0)
            {
                buttons.Add(rowButtons.ToArray());
            }

            buttons.Add(new KeyboardButton[] { new KeyboardButton(backCommand) });

            ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup(buttons.ToArray())
            {
                ResizeKeyboard = true
            };
            return markup;
        }
        
        private ReplyKeyboardMarkup ProductsMarkup(IQueryable<Product> products)
        {
            var buttons = new List<KeyboardButton[]>();
            List<KeyboardButton> rowButtons = new List<KeyboardButton>();
            buttons.Add(new KeyboardButton[] { new KeyboardButton(basketCommand) });

            foreach (var product in products)
            {
                rowButtons.Add(new KeyboardButton($"{product.Name}"));

                if (rowButtons.Count == 3)
                {
                    buttons.Add(rowButtons.ToArray());
                    rowButtons.Clear();
                }
            }

            if (rowButtons.Count > 0)
            {
                buttons.Add(rowButtons.ToArray());
            }

            buttons.Add(new KeyboardButton[] { new KeyboardButton(backCommand) });

            ReplyKeyboardMarkup markup = new ReplyKeyboardMarkup(buttons.ToArray())
            {
                ResizeKeyboard = true
            };
            return markup;
        }

        private static ReplyKeyboardMarkup ProductMarkup()
        {
            var keyboardButtons = new List<KeyboardButton[]>();
            keyboardButtons.Add(new KeyboardButton[] { new KeyboardButton(basketCommand) });

            for (int i = 1; i <= 9; i += 3)
            {
                var rowButtons = new List<KeyboardButton>();

                for (int j = 0; j < 3 && i + j <= 9; j++)
                {
                    rowButtons.Add(new KeyboardButton((i + j).ToString()));
                }

                keyboardButtons.Add(rowButtons.ToArray());
            }

            keyboardButtons.Add(new KeyboardButton[]
            {
                new KeyboardButton("⬅️ Back")
            });

            return new ReplyKeyboardMarkup(keyboardButtons)
            {
                ResizeKeyboard = true
            };
        }

        private static ReplyKeyboardMarkup BasketMarkup(IQueryable<Order> orders)
        {
            var keyboardButtons = new List<KeyboardButton[]>();

            foreach (var order in orders)
            {
                var orderButton = new KeyboardButton($"❌ {order.Name}");
                keyboardButtons.Add(new KeyboardButton[] { orderButton });
            }

            keyboardButtons.Add(new KeyboardButton[]
            {
                new KeyboardButton("⬅️ Back"),
                new KeyboardButton("🔄 Clear"),
                
            });

            keyboardButtons.Add(new KeyboardButton[] { "Place an order ✅" });

            return new ReplyKeyboardMarkup(keyboardButtons)
            {
                ResizeKeyboard = true
            };
        }
    }
}
