import { ProductModel } from "./product.model";
import { RecipeModel } from "./recipe.model";

export class RequirementsPlanningModel{
    date: string = "";
    title: string = "";
    products: ProductModel[]=[];
    recipe: RecipeModel[]=[];
    hasRecipe: boolean = true; // Backend'den gelen değeri tutacak

}