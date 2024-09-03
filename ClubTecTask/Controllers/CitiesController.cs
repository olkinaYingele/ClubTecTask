using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ClubTecTask.Models;
using Microsoft.EntityFrameworkCore;
using System;
using ClubTecTask.Data;
using ClubTecTask.Utilities;

namespace ClubTecTask.Controllers;

public class CitiesController : Controller
{
    private readonly CitiesListContext _context;

    public CitiesController(CitiesListContext context)
    {
        _context = context;
    }


    // Метод для отображения списка городов с возможностью добавления и удаления
    public async Task<IActionResult> Index(int? pageNumber)
    {
        int pageSize = 5;

        var cities = from s in _context.Locations select s;

        return View(await PaginatedList<CitiesList>.CreateAsync(cities.AsNoTracking(), pageNumber ?? 1, pageSize));
    }


    // Метод для добавления нового города
     [HttpPost]
    public async Task<IActionResult> AddCity(string cityName)
    {
        if (!string.IsNullOrEmpty(cityName))
        {
            // Проверяем, существует ли город с таким же названием
            bool cityExists = _context.Locations.Any(c => c.Name == cityName);

            if (cityExists)
            {
                // Если город уже существует, возвращаем сообщение об ошибке
                ModelState.AddModelError(string.Empty, "יישוב עם שם זה כבר קיים.");
                return View("Index", await PaginatedList<CitiesList>.CreateAsync(_context.Locations.AsNoTracking(), 1, 5));
            }

            // Если город не существует, добавляем его
            _context.Locations.Add(new CitiesList { Name = cityName });
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }


    // Метод для удаления города
    [HttpPost]
    public async Task<IActionResult> DeleteCity(int id)
    {
        var city = await _context.Locations.FindAsync(id);
        if (city != null)
        {
            _context.Locations.Remove(city);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

	[HttpPost]
	public IActionResult Edit(int id)
	{
		ViewData["EditId"] = id;
		return View("Index", PaginatedList<CitiesList>.CreateAsync(_context.Locations.AsNoTracking(), 1, 5).Result);
	}

	[HttpPost]
	public async Task<IActionResult> EditCity(int id, string cityName)
	{
		if (!string.IsNullOrEmpty(cityName))
		{
			var city = await _context.Locations.FindAsync(id);
			if (city != null)
			{
				city.Name = cityName;
				_context.Update(city);
				await _context.SaveChangesAsync();
			}
		}
		return RedirectToAction(nameof(Index));
	}

}
