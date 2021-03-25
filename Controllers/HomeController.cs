using HiltonMovies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HiltonMovies.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MoviesDbContext Context { get; set; }

        public HomeController(ILogger<HomeController> logger, MoviesDbContext _context)
        {
            _logger = logger;
            Context = _context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MyMovies()
        {
            return View(Context.Movies);
        }
        public IActionResult MyPodcast()
        {
            return View();
        }
        [HttpGet]
        public IActionResult NewMovie()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewMovie(Movie newMovie)
        {
            // Check for null values
            if (newMovie.LentTo == null)
            {
                newMovie.LentTo = "-";
            }
            if (newMovie.Notes == null)
            {
                newMovie.Notes = "-";
            }
            // Checks model state and adds the movie to DB
            if (ModelState.IsValid)
            {
                // Add the newMovie to the DB
                Context.Movies.Add(newMovie);
                Context.SaveChanges();
            }
            // Redirects to the MyMovies Page so that the model can be correctly passed in
            return RedirectToAction("MyMovies");
        }
        [HttpGet]
        public IActionResult EditMovie(int movieID)
        {
            Movie movieToEdit = Context.Movies.FirstOrDefault(movie => movie.MovieID == movieID);
           
            return View(movieToEdit);
        }
        [HttpPost]
        public IActionResult EditMovie(Movie updatedMovie)
        {
            // Find the movie in the context that matches the movie object coming into this controller function
            Movie movieToEdit = Context.Movies.FirstOrDefault(movie => movie.MovieID == updatedMovie.MovieID);
            // Updates the various values of movie in the database to match the movie object info coming in
            movieToEdit.Category = updatedMovie.Category;
            movieToEdit.Title = updatedMovie.Title;
            movieToEdit.Year = updatedMovie.Year;
            movieToEdit.Director = updatedMovie.Director;
            movieToEdit.Rating = updatedMovie.Rating;
            movieToEdit.Edited = updatedMovie.Edited;
            
            // Check for null values on the two non-required fields
            if (updatedMovie.LentTo == null)
            {
                movieToEdit.LentTo = "-";
            } else
            {
                movieToEdit.LentTo = updatedMovie.LentTo;
            }
            if (updatedMovie.Notes == null)
            {
                movieToEdit.Notes = "-";
            }
            else
            {
                movieToEdit.Notes = updatedMovie.Notes;
            }
            // Checks model state prior to updating the movie to DB
            if (ModelState.IsValid)
            {
                // Save the changes
                Context.SaveChanges();
            }
            return RedirectToAction("MyMovies");
        }

        // Deletes the selected Movie
        [HttpPost]
        public IActionResult DeleteMovie(int movieID)
        {
            // Finds the movie to delete given the movieID brought in (name attribute is neccessary) 
            Movie movieToDelete = Context.Movies.FirstOrDefault(movie => movie.MovieID == movieID);
            Context.Movies.Remove(movieToDelete);
            Context.SaveChanges();
            return RedirectToAction("MyMovies");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
