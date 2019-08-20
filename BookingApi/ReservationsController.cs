using Microsoft.AspNetCore.Http;
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
            IMaîtreD maîtreD,
            IReservationsRepository repository)
        {
            Validator = validator;
            Mapper = mapper;
            MaîtreD = maîtreD;
            Repository = repository;
        }

        public IValidator Validator { get; }
        public IMapper Mapper { get; }
        public IMaîtreD MaîtreD { get; }
        public IReservationsRepository Repository { get; }

        public ActionResult Post(ReservationDto dto)
        {
            var validationMsg = Validator.Validate(dto);
            if (validationMsg != "")
                return BadRequest(validationMsg);

            var reservation = Mapper.Map(dto);
            try
            {
                MaîtreD.TryAccept(reservation);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Couldn't accept.");
            }

            var id = Repository.ReadReservationId(reservation.Id);
            return Ok(id);
        }
    }
}
