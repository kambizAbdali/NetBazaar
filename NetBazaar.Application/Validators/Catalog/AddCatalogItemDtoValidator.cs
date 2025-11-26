//using FluentValidation;
//using NetBazaar.Application.DTOs.Catalog;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace NetBazaar.Application.Validators.Catalog
//{
//    public class AddCatalogItemDtoValidator : AbstractValidator<AddCatalogItemDto>
//    {
//        public AddCatalogItemDtoValidator()
//        {
//            RuleFor(x => x.Name)
//                .NotEmpty().WithMessage("نام محصول اجباری است")
//                .Length(2, 100).WithMessage("نام محصول باید بین ۲ تا ۱۰۰ کاراکتر باشد");

//            RuleFor(x => x.Description)
//                .NotEmpty().WithMessage("توضیحات محصول اجباری است");

//            RuleFor(x => x.Price)
//                .GreaterThan(0).WithMessage("قیمت باید بزرگتر از صفر باشد");

//            RuleFor(x => x.StockQuantity)
//                .InclusiveBetween(0, 100000).WithMessage("تعداد موجودی باید بین ۰ تا ۱۰۰,۰۰۰ باشد");
//        }
//    }
//}
