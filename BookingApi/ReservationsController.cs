﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploeh.Samples.BookingApi
{
    [ApiController, Route("[controller]")]
    public class ReservationsController : ControllerBase
    {
        public ReservationsController(
            IValidator validator,
            IMapper mapper,
            IReservationsRepository repository,
            int capacity)
        {
            Validator = validator;
            Mapper = mapper;
            Repository = repository;
            Capacity = capacity;
        }

        public IValidator Validator { get; }
        public IMapper Mapper { get; }
        public IReservationsRepository Repository { get; }
        public int Capacity { get; }

        public ActionResult Post(ReservationDto dto)
        {
            var validationMsg = Validator.Validate(dto);
            if (validationMsg != "")
                return BadRequest(validationMsg);

            var reservation = Mapper.Map(dto);
            var reservations = Repository.ReadReservations(reservation.Date);

            var maîtreD = new MaîtreD(Capacity);
            var accepted = maîtreD.CanAccept(reservations, reservation);
            if (!accepted)
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Couldn't accept.");
            var id = Repository.Create(reservation);
            return Ok(id);
        }
    }
}
