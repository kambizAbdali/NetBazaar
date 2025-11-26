using NetBazaar.Domain.Entities.Catalog;
using NetBazaar.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Persistence.Data.Seed
{
    public static class SeedData
    {
        public static void SeedCatalogTypes(NetBazaarDbContext context)
        {
            if (!context.CatalogTypes.Any())
            {
                // کالای دیجیتال
                var digital = new CatalogType { Type = "📱 کالای دیجیتال" };
                digital.Children.Add(new CatalogType { Type = "📱 گوشی‌های هوشمند", Parent = digital });
                digital.Children.Add(new CatalogType { Type = "💻 تبلت و لپ‌تاپ", Parent = digital });
                digital.Children.Add(new CatalogType { Type = "📸 دوربین و لوازم جانبی", Parent = digital });
                digital.Children.Add(new CatalogType { Type = "📺 صوتی و تصویری", Parent = digital });
                digital.Children.Add(new CatalogType { Type = "💾 فضای ذخیره‌سازی و شبکه", Parent = digital });

                // خانه و آشپزخانه
                var home = new CatalogType { Type = "🏠 خانه و آشپزخانه" };
                home.Children.Add(new CatalogType { Type = "🔌 لوازم برقی", Parent = home });
                home.Children.Add(new CatalogType { Type = "🔪 ابزار آشپزی", Parent = home });
                home.Children.Add(new CatalogType { Type = "⚡ لوازم خانگی کوچک", Parent = home });

                // سلامت و زیبایی
                var health = new CatalogType { Type = "💄 سلامت و زیبایی" };
                health.Children.Add(new CatalogType { Type = "✨ مراقبت پوست", Parent = health });
                health.Children.Add(new CatalogType { Type = "🏥 بهداشت و سلامت", Parent = health });
                health.Children.Add(new CatalogType { Type = "💅 زیبایی و آرایش", Parent = health });

                // مد و پوشاک
                var fashion = new CatalogType { Type = "👕 مد و پوشاک" };
                fashion.Children.Add(new CatalogType { Type = "👔 مردانه", Parent = fashion });
                fashion.Children.Add(new CatalogType { Type = "👗 زنانه", Parent = fashion });
                fashion.Children.Add(new CatalogType { Type = "👶 بچه‌گانه", Parent = fashion });
                fashion.Children.Add(new CatalogType { Type = "👠 کفش و اکسسوری", Parent = fashion });

                // کودک و نوزاد
                var baby = new CatalogType { Type = "🍼 کودک و نوزاد" };
                baby.Children.Add(new CatalogType { Type = "🧸 Toys و سرگرمی", Parent = baby });
                baby.Children.Add(new CatalogType { Type = "🥣 لوازم غذایی کودک", Parent = baby });
                baby.Children.Add(new CatalogType { Type = "📚 کتب و سرگرمی", Parent = baby });

                // ورزش و سفر
                var sport = new CatalogType { Type = "⚽ ورزش و سفر" };
                sport.Children.Add(new CatalogType { Type = "🏃 تجهیزات ورزشی", Parent = sport });
                sport.Children.Add(new CatalogType { Type = "🎒 تجهیزات سفر", Parent = sport });
                sport.Children.Add(new CatalogType { Type = "👕 پوشاک ورزشی", Parent = sport });

                // خانه‌داری و باغبانی
                var gardening = new CatalogType { Type = "🌷 خانه‌داری و باغبانی" };
                gardening.Children.Add(new CatalogType { Type = "🌿 ابزار باغبانی", Parent = gardening });
                gardening.Children.Add(new CatalogType { Type = "🧹 نحوه نظافت منزل", Parent = gardening });

                // خودرو و موتور
                var car = new CatalogType { Type = "🚗 خودرو و موتور" };
                car.Children.Add(new CatalogType { Type = "🔧 لوازم جانبی خودرو", Parent = car });
                car.Children.Add(new CatalogType { Type = "🛠️ ابزار و نگهداری وسیله نقلیه", Parent = car });

                // کتاب، فیلم و موسیقی
                var media = new CatalogType { Type = "🎵 کتاب، فیلم و موسیقی" };
                media.Children.Add(new CatalogType { Type = "📖 کتاب‌های چاپی و کتاب‌های صوتی", Parent = media });
                media.Children.Add(new CatalogType { Type = "🎬 فیلم و سریال", Parent = media });
                media.Children.Add(new CatalogType { Type = "🎸 موسیقی و ابزار موسیقی", Parent = media });

                // نرم‌افزار و دیجیتال
                var software = new CatalogType { Type = "💻 نرم‌افزار و دیجیتال" };
                software.Children.Add(new CatalogType { Type = "📱 نرم‌افزارهای کاربردی", Parent = software });
                software.Children.Add(new CatalogType { Type = "💿 گیگابایت و رم/دیسک‌های جانبی", Parent = software });

                // اضافه کردن همه به دیتابیس
                context.CatalogTypes.AddRange(
                    digital, home, health, fashion, baby, sport, gardening, car, media, software
                );

                context.SaveChanges();
            }
        }
    }

}
