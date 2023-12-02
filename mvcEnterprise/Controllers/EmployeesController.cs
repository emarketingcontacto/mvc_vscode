using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvcEnterprise.Data;
using mvcEnterprise.Models;

namespace mvcEnterprise.Controllers; 

public class EmployeesController : Controller{

	//AppDBContext db = new AppDBContext(); 
	private AppDBContext _context;

	public EmployeesController(AppDBContext context){
		_context = context; 
	}

	
	/*public ActionResult Index(){
		var employees = _context.Employees; 
		//var employees = _context.Employees.Include(e => e.Categories); 

		//var employees = _context.Employees; 
		//return View(employees.ToList()); 
	}
	*/

	public async Task<IActionResult> Index(){
		try
		{

            //var employees = await _context.Employees.Include(x => x.Categories).ToListAsync(); 
            //  var categories = await _context.Categories.ToListAsync();
            /*
            var categories = await _context.Categories.ToListAsync();
            SelectList sl = new SelectList(categories, "catId", "catName", employees);
            TempData["CategoriesList"] = sl;
            TempData.Keep();
			*/

            var employees = await _context.Employees.ToListAsync();
            var categories = await _context.Categories.ToListAsync();

          
			IList<EmployeeCategoryModel> modelList = new List<EmployeeCategoryModel>();

            foreach (var employee in employees)
			{
                EmployeeCategoryModel model = new EmployeeCategoryModel
                {
                    empId = employee.empId,
                    empName = employee.empName,
                    catId = employee.catId
                };
				
                var findCat = categories.FirstOrDefault(x => x.catId == employee.catId);
				if(findCat != null)
				{
                	model.catName = findCat.catName.ToString();
                }else { 
					throw new InvalidOperationException("No catName");  }

				modelList.Add(model); 
            }
           // var categories = await _context.Categories.ToListAsync();

		//	var EmployeeCategorieViewModel = new 

            return View(modelList);
        }
		catch (Exception ex)
		{
            ModelState.AddModelError(string.Empty, $"Error {ex.Message}");
        }
        ModelState.AddModelError(string.Empty, $"Error Invalid Model");
        return View("index");
    }

	[HttpGet]
	public async Task<IActionResult> Details(int id)
	{
		//	Employee exist =  _context.Employees.FirstOrDefault(x => x.empId == id); 
		// Employee deleteEmployee = _context.Employees.Where(x => x.empId == id).FirstOrDefault();

		createTempData(); 
		var details = await _context.Employees.Where(x=>x.empId == id).FirstOrDefaultAsync();
		return View(details);

	}


	[HttpGet]
	public IActionResult Create(){

		createTempData(); 
		return View(); 
	}

	[HttpPost]
	public async Task<IActionResult> Create(Employee employee){
		if(ModelState.IsValid)
		{
			try
			{
				_context.Add(employee);
				await _context.SaveChangesAsync();
				return RedirectToAction("index"); 
			}
			catch (Exception ex) 
			{
				ModelState.AddModelError(string.Empty, $"Error {ex.Message}"); 
				
			}
		}
		
		ModelState.AddModelError(string.Empty, $"Error Invalid Model");
		return View(employee);
	}

	[HttpGet]
	public async Task<IActionResult> Edit (int id){

		createTempData(); 
		var exist = await _context.Employees.Where(x => x.empId == id).FirstOrDefaultAsync();
		return View(exist); 
	}

	[HttpPost]
	public async Task<IActionResult> Edit(Employee employee){
		if(ModelState.IsValid){
			try
			{
				var exist = _context.Employees.Where(x => x.empId == employee.empId).FirstOrDefault(); 
				if(exist != null){
					exist.empName = employee.empName;
					exist.empLName = employee.empLName;
					exist.catId = employee.catId; 
				}
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");  
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, $"Error {ex.Message}"); 
			}
		}
		ModelState.AddModelError(string.Empty, $"Error Invalid Model");
		return View(employee); 
	}

	[HttpGet]
	public async Task<IActionResult>Delete(int id){
		createTempData(); 
		var exist = await _context.Employees.Where(x => x.empId == id).FirstOrDefaultAsync();
		return View(exist); 
	}

	[HttpPost]
	public async Task<IActionResult>Delete(Employee employee){
		if(ModelState.IsValid){
			try
			{
				var exist = _context.Employees.Where(x => x.empId == employee.empId).FirstOrDefault();
				if (exist != null) {
					_context.Remove(exist);
					await _context.SaveChangesAsync();
					return RedirectToAction("Index"); 
				}
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, $"Error {ex.Message}"); 
			}
		}
			ModelState.AddModelError(string.Empty, $"Error Invalid Model");
		return View(employee); 
	}


	public void createTempData()
	{
		TempData.Clear(); 
		List<Category> categories = [.. _context.Categories];
		SelectList categoriesList = new SelectList(categories, "catId", "catName");
		TempData["CategoriesList"] = categoriesList;
	}
}