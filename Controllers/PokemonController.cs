﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon.Services;
using Pokemon.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pokemon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        /// <summary>
        /// Returns standard Pokemon description and additional information.
        /// </summary>
        /// <param name="name">Pokemon's name.</param>
        /// <returns>Information about the pokemon.</returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PokemonResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{name}")]
        // I don't think that pokemon stats change really often
        // or someone can die if they don't receive the update within 30 minutes
        // so we can cache the response in the browser.
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 1800)]
        public async Task<IActionResult> Get([FromRoute] string name, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Incorrect name. Please specify non empty pokemon name.");
            }
            // https://pokeapi.co/api/v2/pokemon/ditto
            var pokemon = await _pokemonService.GetPokemonOrNullAsync(name, token);
            if (pokemon == null)
            {
                return NotFound("There is no pokemon with the name you provided.");
            }

            return Ok(pokemon);
        }
    }
}