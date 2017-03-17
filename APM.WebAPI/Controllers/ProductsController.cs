using APM.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.OData;

namespace APM.WebAPI.Controllers
{
    [EnableCorsAttribute("http://localhost:55547", "*", "*")]
    public class ProductsController : ApiController
    {
        // GET: api/Products
        [EnableQuery()]
        [ResponseType(typeof(Product))]
        public IHttpActionResult Get()
        {
            try
            {
                var ProductRepository = new ProductRepository();
                return Ok(ProductRepository.Retrieve().AsQueryable());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        [Authorize()]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Product product;
                var ProductRepository = new ProductRepository();

                if (id > 0)
                {
                    var products = ProductRepository.Retrieve();
                    product = products.FirstOrDefault(p => p.ProductId == id);
                    if (product == null)
                    {
                        return NotFound();
                    }
                }
                else
                {
                    product = ProductRepository.Create();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult Post([FromBody]Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Product can not be null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var productRepository = new Models.ProductRepository();
                var newProduct = productRepository.Save(product);
                if (newProduct == null)
                {
                    return Conflict();
                }
                return Created<Product>(Request.RequestUri + newProduct.ProductId.ToString(), newProduct);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult Put(int id, [FromBody]Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Product can not be null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var productRepository = new Models.ProductRepository();
                var updatedProduct = productRepository.Save(id, product);
                if (updatedProduct == null)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/Products/5
        public void Delete(int id)
        {
        }
    }
}
