import { Component, ElementRef, ViewChild } from '@angular/core';
import { DepotModel } from '../../models/depot.model';
import { ProductModel } from '../../models/product.model';
import { ProductionModel } from '../../models/production.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { NgForm } from '@angular/forms';
import { SharedModule } from '../../modules/shared.module';
import { ProductionPipe } from '../../pipes/production.pipe';

@Component({
  selector: 'app-productions',
  imports: [SharedModule, ProductionPipe],
  templateUrl: './productions.component.html',
  styleUrl: './productions.component.css'
})
export class ProductionsComponent {
  depots: DepotModel[] = [];
  products: ProductModel[] = [];
  createModel: ProductionModel= new ProductionModel();

  productions: ProductionModel[] = [];

  search: string = "";

@ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined;


 constructor(private http: HttpService, private swal: SwalService){}

 ngOnInit(): void {
   this.getAll();
   this.getAllProducts();
   this.getAllDepots();
 }

 getAll(){
   this.http.post<ProductionModel[]>("Productions/GetAll",{},(res)=> {
     this.productions= res;
   })
 }

 getAllProducts(){
  this.http.post<ProductModel[]>("Products/GetAll",{},(res)=> {
    this.products= res;
  })
}

getAllDepots(){
  this.http.post<DepotModel[]>("Depots/GetAll",{},(res)=> {
    this.depots= res;
  })
}

 create(form: NgForm){
   if(form.valid){
     this.http.post<string>("Productions/Create",this.createModel,(res)=> {
       this.swal.callToast(res);
       this.createModel= new ProductionModel();
       this.createModalCloseBtn?.nativeElement.click();
       this.getAll();
     })
   }
 }

 deleteById(model: ProductionModel){
   this.swal.callSwal("Üretimi Sil?", `${model.product.name} üretimini silmek istiyor musunuz?`, ()=> {
         this.http.post<string>("Productions/DeleteById", {id: model.id},(res)=> {
         this.getAll();
         this.swal.callToast(res,"info");
        });
 })
}
}
