using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using VirtualBazaar.Core.Models.Foundations.Categories;
using VirtualBazaar.Core.Models.Foundations.Products;

namespace VirtualBazaar.Core.Services.Orchestrations.Admins
{
    public partial class AdminOrchestrationService
    {
        private static ReplyKeyboardMarkup ContactMarkup()
        {
            return new ReplyKeyboardMarkup(new KeyboardButton[][]
            {
               new KeyboardButton[]{ new KeyboardButton("Share contact 📱") { RequestContact = true } },
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
                    new KeyboardButton("Categories 🛍")
                },
                new KeyboardButton[]
                {
                    new KeyboardButton("Me 👤"),
                    new KeyboardButton("Settings ⚙️"),
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
                    new KeyboardButton("Change organization name 🪄"),
                    new KeyboardButton("Change organization address 🏡"),
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
            buttons.Add(new KeyboardButton[] { new KeyboardButton("Add new category ➕") });

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
            buttons.Add(new KeyboardButton[] { new KeyboardButton("Add new product ➕") });

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
            var keyboardButtons = new List<KeyboardButton[]>
            {
                new KeyboardButton[]
                {
                    new KeyboardButton("Delete product ❌")
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
    }
}
