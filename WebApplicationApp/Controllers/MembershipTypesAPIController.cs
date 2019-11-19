using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplicationApp.Models;

namespace WebApplicationApp.Controllers
{
    public class MembershipTypesAPIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/MembershipTypesAPI
        public IQueryable<MembershipType> GetMemberships()
        {
            return db.Memberships;
        }

        // GET: api/MembershipTypesAPI/5
        [ResponseType(typeof(MembershipType))]
        public IHttpActionResult GetMembershipType(byte id)
        {
            MembershipType membershipType = db.Memberships.Find(id);
            if (membershipType == null)
            {
                return NotFound();
            }

            return Ok(membershipType);
        }

        //// GET: api/MembershipTypesAPI/5
        //[ResponseType(typeof(MembershipType))]
        //public IHttpActionResult GetMembershipType(string name)
        //{
        //    MembershipType membershipType = db.Memberships.Find(id);
        //    if (membershipType == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(membershipType);
        //}

        // PUT: api/MembershipTypesAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMembershipType(byte id, MembershipType membershipType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != membershipType.Id)
            {
                return BadRequest();
            }

            db.Entry(membershipType).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembershipTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/MembershipTypesAPI
        [ResponseType(typeof(MembershipType))]
        public IHttpActionResult PostMembershipType(MembershipType membershipType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Memberships.Add(membershipType);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (MembershipTypeExists(membershipType.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = membershipType.Id }, membershipType);
        }

        // DELETE: api/MembershipTypesAPI/5
        [ResponseType(typeof(MembershipType))]
        public IHttpActionResult DeleteMembershipType(byte id)
        {
            MembershipType membershipType = db.Memberships.Find(id);
            if (membershipType == null)
            {
                return NotFound();
            }

            db.Memberships.Remove(membershipType);
            db.SaveChanges();

            return Ok(membershipType);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MembershipTypeExists(byte id)
        {
            return db.Memberships.Count(e => e.Id == id) > 0;
        }
    }
}