﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Domain.Entities.Enums
{
    public enum BusinessErrors
    {
        EmailAlreadyRegistered,
        EntityNotFoundInDataBase,
        ErrorOnSaveChangesInDatabase,
        ThereNoActiveMoviesInCommand,

        FailToCreateMovie,
        FailToUpdateMovie,
        FailToDeleteMovie,
        FailToDeleteMovies,
        MovieGenreIsInactive,
        FailToCreateGenre,

        FailToCreateMoviesRent,
        EmailNotFound,
        PasswordIncorrect,
        MovieNotFound
    }
}
