using System;
using System.Linq;

namespace WebDoctorAppointment.Models;

public class PageViewModel
{
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public int MaxShowPages { get; }

    public PageViewModel(int count, int pageNumber, int pageSize, int maxPages = 7)
    {
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        if(pageNumber < 1)
            pageNumber = 1;
        else if(pageNumber > TotalPages)
            pageNumber = TotalPages;
        PageNumber = pageNumber;
        MaxShowPages = maxPages;
    }

    public bool HasPreviousPage => (PageNumber > 1);
    public bool HasNextPage => (PageNumber < TotalPages);
    public int[] Pages => GetPages(TotalPages, PageNumber);

    private static int[] GetPages(int totalPages, int page, int width = 7)
    {
        const int minPageCount = 7;

        if (width < minPageCount)
            throw new Exception($"Must allow at least ${minPageCount} page items");

        if (width % 2 == 0)
            throw new Exception("Must allow odd number of page items");

        var halfWidth = width / 2;
        if (totalPages < width)
            return Enumerable.Range(1, totalPages).ToArray();

        var left = Math.Max(1, page - halfWidth);
        var right = Math.Min(totalPages, page + halfWidth);

        if (left == 1)
            right = left + width - 1;

        if (right == totalPages)
            left = right - width + 1;

        var result = Enumerable.Range(left, right - left + 1).ToArray();

        var idx = 0;
        if (result[idx] > 1)
        {
            result[idx] = 1;
            // ReSharper disable once UselessBinaryOperation
            result[idx + 1] = -1;
        }

        idx = result.Length - 1;
        if (result[idx] < totalPages)
        {
            result[idx] = totalPages;
            result[idx - 1] = -1;
        }

        return result;
    }
}