﻿using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AccountOwnerServer.Controllers
{
    [Route("api/owner")]
    public class OwnerController : Controller
    {
        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;

        public OwnerController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAllOwners()
        {
            try
            {
                var owners = _repository.Owner.GetAllOwners();

                _logger.LogInfo($"Returned all owners from database.");

                return Ok(owners);
            }

            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "OwnerById")]
        public IActionResult GetOwnerById(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerById(id);

                if(owner.Id == Guid.Empty)
                {
                    _logger.LogError($"Owner with id: {id}, was not found.");
                    return NotFound();
                }
                
                _logger.LogInfo($"Returned owner with id: {id}");
                return Ok(owner);
                
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong inside the GetOwnerById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}/account")]
        public IActionResult GetOwnerWithDetails(Guid id)
        {
            try
            {
                var owner = _repository.Owner.GetOwnerWithDetails(id);

                if(owner.Id == Guid.Empty)
                {
                    _logger.LogError($"Owner with id: {id}, was not found.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned owner details of id: {id}.");
                return Ok(owner);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetOwnerWithDetails action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        public IActionResult CreateOwner([FromBody]Owner owner)
        {
            try
            {
                if(owner == null)
                {
                    _logger.LogError("Owner object from client is null.");
                    return BadRequest("Owner object is null");
                }

                if(!ModelState.IsValid)
                {
                    _logger.LogError("Invalid owner object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.Owner.CreateOwner(owner);

                return CreatedAtRoute("OwnerById", new { id = owner.Id }, owner);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateOwner action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
