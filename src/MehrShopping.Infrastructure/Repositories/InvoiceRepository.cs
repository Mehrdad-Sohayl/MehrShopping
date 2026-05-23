using MehrShopping.Application.Common;
using MehrShopping.Application.Dtos;
using MehrShopping.Application.Interfaces;
using MehrShopping.Application.Services.Invoice.Queries;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Interfaces.Repositories;
using MehrShopping.Infrastructure.Common;
using MehrShopping.Infrastructure.Data;
using MehrShopping.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MehrShopping.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository, IInvoiceReadRepository
    {
        private readonly MehrShoppingDbContext _context;

        public InvoiceRepository(MehrShoppingDbContext context)
        {
            _context = context;
        }

        public IQueryable<Invoice> Table => throw new NotImplementedException();

        public async Task AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
        }

        public Task DeleteAsync(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Invoice>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Invoice>> GetByCustomerIdAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<Invoice?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<List<InvoiceListDto>>> GetInvoiceListAsync(InvoiceListRequest request)
        {
            IQueryable<Invoice> query =
                _context
                .Invoices
                .Include(i => i.Items)
                .ThenInclude(ii => ii.Product)
                .AsNoTracking();

            query = ApplyFiltersAndSort(query, request);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .Select(i => new InvoiceListDto(
                    i.Customer.FirstName.Value,
                     i.Customer.LastName.Value,
                     i.Customer.NationalCode.Value,
                     i.Items.Select(ii =>
                        new InvoiceItemListDto(
                             ii.Product.Name.Value,
                             ii.Quantity.Value)).ToList())).ToListAsync();

            return PagedResult<List<InvoiceListDto>>.Success(items, request.Page, request.Size, totalCount);
        }

        private static IQueryable<Invoice> ApplyFiltersAndSort(IQueryable<Invoice> query, InvoiceListRequest request)
        {
            foreach (var filter in request.Filters)
            {
                query = ApplyFilter(query, filter);
            }

            return query;
        }

        private static IQueryable<Invoice> ApplyFilter(IQueryable<Invoice> query, InvoiceFilter filter)
        {
            return filter.Field switch
            {
                InvoiceFilterField.CustomerFirstName =>
                ApplyStringFilter(query, i => i.Customer.FirstName.Value, filter),
                InvoiceFilterField.CustomerLastName =>
                ApplyStringFilter(query, i => i.Customer.LastName.Value, filter),
                InvoiceFilterField.CustomerNationalCode =>
                ApplyStringFilter(query, i => i.Customer.NationalCode.Value, filter),
                InvoiceFilterField.ProductName =>
                ApplyItemProductNameFilter(query, filter),
                _ => throw new InfrastructureException(new InfrastructureError(InfrastructureErrorCodes.InvalidFilter, nameof(filter.Field)))
            };
        }


        private static IQueryable<Invoice> ApplyStringFilter(
            IQueryable<Invoice> query,
            Expression<Func<Invoice, string>> selector,
            InvoiceFilter filter)
        {
            return filter.Operator switch
            {
                FilterOperator.Equals => query.Where(BuildEquals(selector, filter.Value)),
                FilterOperator.Contains => query.Where(BuildContains(selector, filter.Value)),
                _ => throw new InvalidOperationException($"Operator {filter.Operator} is not supported.")
            };
        }

        private static IQueryable<Invoice> ApplyItemProductNameFilter(IQueryable<Invoice> query, InvoiceFilter filter)
        {
            return filter.Operator switch
            {
                FilterOperator.Equals =>
                query.Where(i => i.Items.Any(ii => ii.Product.Name.Value == filter.Value)),
                FilterOperator.Contains =>
                query.Where(i => i.Items.Any(ii => ii.Product.Name.Value.Contains(filter.Value))),
                _ => throw new InvalidOperationException($"Operator {filter.Operator} is not supported.")
            };
        }

        private static Expression<Func<Invoice, bool>> BuildEquals(
            Expression<Func<Invoice, string>> selector,
            string value)
        {
            var parameter = selector.Parameters[0];
            var body = Expression.Equal(selector.Body, Expression.Constant(value));
            return Expression.Lambda<Func<Invoice, bool>>(body, parameter);
        }

        private static Expression<Func<Invoice, bool>> BuildContains(
            Expression<Func<Invoice, string>> selector,
            string value)
        {
            var parameter = selector.Parameters[0];
            var body = Expression.Call(
                selector.Body,
                nameof(string.Contains),
                Type.EmptyTypes,
                Expression.Constant(value));

            return Expression.Lambda<Func<Invoice, bool>>(body, parameter);
        }
    }
}
