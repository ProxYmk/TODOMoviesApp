using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movies.Models;

namespace Movies.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public async Task<IActionResult> Index(int movieGenre, string searchString)
        {
            var GenreList = _context.Genre.Select(g => new
            {
                GenreID = g.ID,
                GenreName = g.Name
            });

            var movies = from m in _context.Movie
                         join g in _context.Genre on m.GenreID equals g.ID
                         select new { g, m };

            var result = await movies.ToListAsync();

            List<Movie> movieList = new List<Movie>();
            foreach (var item in result)
            {
                movieList.Add(item.m);
            }


            if (!String.IsNullOrEmpty(searchString))
            {
                movieList = (List<Movie>)movieList
                    .Where(m => m.Title.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }

            if (movieGenre != 0)
            {
                movieList = (List<Movie>)movieList
                    .Where(m => m.Genre.ID == movieGenre)
                    .ToList();
            }

            var movieGenreVM = new MovieIndexViewModel();
            movieGenreVM.genres = new SelectList(GenreList, "GenreID", "GenreName");
            movieGenreVM.movies = movieList;

            return View(movieGenreVM);
        }

        // GET: Movies/Details/ ID
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .SingleOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            var actorsList = _context.Actor.Select(a => new
            {
                ActorID = a.ID,
                ActorName = a.Name
            });

            var movieViewModel = new MovieViewModel();
            movieViewModel.ActorList = new MultiSelectList(actorsList, "ActorID", "ActorName");
            movieViewModel.Movie = new Movie();
            movieViewModel.Movie.Title = "ABC";
            movieViewModel.Movie.GenreID = 1;
            movieViewModel.Movie.Price = 10;
            movieViewModel.Movie.Rating = "RET";
            return View(movieViewModel);
        }
        //[Bind("Movie_ID,Movie_Title,Movie_ReleaseDate,Movie_Genre,Movie_Price,Movie_Rating,ActorList")]
        //MovieViewModel movieVM

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, MovieViewModel movieVM)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                foreach (var item in movieVM.MovieActors)
                {
                    Movie movieInserted = _context.Movie.Last(m => m.Title == movie.Title);
                    var movieActor = new MovieActor { MovieID = movieInserted.ID, ActorID = item };
                    _context.MovieActor.Add(movieActor);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(movieVM);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (id != movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .SingleOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }
    }
}
