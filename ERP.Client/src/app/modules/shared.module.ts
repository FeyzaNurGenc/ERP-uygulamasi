import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SectionComponent } from '../components/section/section.component';
import { BlankComponent } from '../components/blank/blank.component';
import { FormsModule } from '@angular/forms';
import { FormValidateDirective } from 'form-validate-angular';
import { TrCurrencyPipe } from 'tr-currency';


@NgModule({
  declarations: [],
  imports: [
    BlankComponent,
    SectionComponent,
    CommonModule,
    FormsModule,
    FormValidateDirective,
    TrCurrencyPipe
  ],
  exports: [
    BlankComponent,
    SectionComponent,
    CommonModule,
    FormsModule,
    FormValidateDirective,
    TrCurrencyPipe
  ]
})
export class SharedModule{

}