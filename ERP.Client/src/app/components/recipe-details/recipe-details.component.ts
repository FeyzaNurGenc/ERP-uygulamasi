import { Component, ElementRef, ViewChild } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { RecipeDetailPipe } from '../../pipes/recipe-detail.pipe';
import { RecipeDetailModel } from '../../models/recipe-detail.model';
import { SwalService } from '../../services/swal.service';
import { HttpService } from '../../services/http.service';
import { NgForm } from '@angular/forms';
import { RecipeModel } from '../../models/recipe.model';
import { ProductModel } from '../../models/product.model';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  selector: 'app-recipe-details',
  imports: [SharedModule, RecipeDetailPipe],
  templateUrl: './recipe-details.component.html',
  styleUrl: './recipe-details.component.css'
})
export class RecipeDetailsComponent {
  recipe: RecipeModel = new RecipeModel();
  search: string ="";
  products: ProductModel[] = [];
  recipeId: string= "";
  isUpdateFormActive: boolean = false;

  createModel: RecipeDetailModel = new RecipeDetailModel();
  updateModel: RecipeDetailModel = new RecipeDetailModel();

  //recipeDetails: RecipeDetailModel[] = [];

  constructor(
    private http: HttpService, 
    private swal: SwalService, 
    private activated: ActivatedRoute){

      this.activated.params.subscribe(res=> {
        this.recipeId = res["id"];
        this.getRecipeById();
        this.createModel.recipeId = this.recipeId;
      })
  }


//ngOnInit kullanmadık.
//activedRoute kullanarak id getirdik ve getRecipeById yi çağırdık
  ngOnInit(): void {
    // this.getRecipeById();
    this.getAllProducts();
  }


  getRecipeById(){
    this.http.post<RecipeModel>("RecipeDetails/GetRecipeByIdWithDetails",{recipeId:this.recipeId},(res)=> {
      this.recipe = res;
      
    })
  }

  getAllProducts(){
    this.http.post<ProductModel[]>("Products/GetAll",{},(res)=> {
      console.log("gördüm");
      this.products = res.filter(p => p.type.value == 2);
    })
  }

  create(form: NgForm){
    console.log("Form submit edildi"); // Butona basınca çalışıyor mu?
    if(form.valid){
      this.http.post<string>("RecipeDetails/Create",this.createModel,(res)=> {
        this.swal.callToast(res);
        this.createModel = new RecipeDetailModel();
        this.createModel.recipeId = this.recipeId;
        this.getRecipeById();
      });
    }
  }

  get(model: RecipeDetailModel){
    console.log("Tıklanan veri:", model);
    this.updateModel = {...model};
    this.updateModel.product = this.products.find(p => p.id == this.updateModel.productId) ?? new ProductModel();
    this.isUpdateFormActive = true;
  }

  update(form: NgForm){
    if(form.valid){
      this.http.post<string>("RecipeDetails/Update",this.updateModel,(res)=> {
        this.swal.callToast(res);
        this.getRecipeById();
        this.isUpdateFormActive = false;
      });
    }
  }

  deleteById(model: RecipeDetailModel){
    this.swal.callSwal("Reçetedeki ürün silinsin mi?",`${model.product.name} ürününü silmek istiyor musunuz?`,()=> {
      this.http.post<string>("RecipeDetails/DeleteById",{id: model.id},(res)=> {
        this.getRecipeById();
        this.swal.callToast(res,"info");
      })
    } )
  }
}
