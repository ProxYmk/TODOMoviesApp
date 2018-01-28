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
            //Get information of a Movie and relation with Genre with LINQ
            var movies = from m in _context.Movie
                         join g in _context.Genre on m.GenreID equals g.ID
                         select new { g, m };

            var result = await movies.ToListAsync();

            List<Movie> movieList = new List<Movie>();
            foreach (var item in result)
            {
                movieList.Add(item.m);
            }

            //Filter by Tittle
            if (!String.IsNullOrEmpty(searchString))
            {
                movieList = (List<Movie>)movieList
                    .Where(m => m.Title.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }
            //Filter by Genre
            if (movieGenre != 0)
            {
                movieList = (List<Movie>)movieList
                    .Where(m => m.Genre.ID == movieGenre)
                    .ToList();
            }

            //Create Custome model to handle the View
            var movieGenreVM = new MovieViewModel();
            movieGenreVM.GenresList = new SelectList(getGenreList(), "GenreID", "GenreName");
            movieGenreVM.MovieList = movieList;

            return View(movieGenreVM);
        }

        // GET: Movies/Details/ ID
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Get all information of a Movie and relations
            var movie = await _context.Movie
                                  .Include(m => m.Genre)
                                  .Include(m => m.MovieActors)
                                  .ThenInclude<Movie, MovieActor, Actor>(ma => ma.Actor)
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
            //Create Custome model to handle the View
            var movieViewModel = new MovieViewModel();
            movieViewModel.ActorList = new MultiSelectList(getActorsList(), "ActorID", "ActorName");
            movieViewModel.GenresList = new SelectList(getGenreList(), "GenreID", "GenreName");
            movieViewModel.Movie = new Movie();
          
            return View(movieViewModel);
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie, MovieViewModel movieVM)
        {
            if (ModelState.IsValid)
            {
                movie.GenreID = movieVM.MovieGenre;
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

            //Get all information of a Movie and relations
            var movie = await _context.Movie
                                  .Include(m => m.Genre)
                                  .Include(m => m.MovieActors)
                                  .ThenInclude<Movie, MovieActor, Actor>(ma => ma.Actor)
                                   .SingleOrDefaultAsync(m => m.ID == id);

            if (movie == null)
            {
                return NotFound();
            }

            //Create Custome model to handle the View
            var movieViewModel = new MovieViewModel();
            movieViewModel.GenresList = new SelectList(getGenreList(), "GenreID", "GenreName");
            movieViewModel.Movie = movie;

            return View(movieViewModel);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,ReleaseDate,Price,Rating")]Movie movie, MovieViewModel movieVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    movie.ID = id;
                    movie.GenreID = movieVM.MovieGenre;
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
            //Get all information of a Movie and relations
            var movie = await _context.Movie
                                  .Include(m => m.Genre)
                                  .Include(m => m.MovieActors)
                                  .ThenInclude<Movie, MovieActor, Actor>(ma => ma.Actor)
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

        private IQueryable<object> getGenreList()
        {  
            //get genre list
            var genreList = _context.Genre.Select(g => new
            {
                GenreID = g.ID,
                GenreName = g.Name
            });
            return genreList;
        }

        private IQueryable<object> getActorsList()
        {
            //get actors list
            var actorsList = _context.Actor.Select(a => new
            {
                ActorID = a.ID,
                ActorName = a.Name
            });
            return actorsList;
        }
    }
}
