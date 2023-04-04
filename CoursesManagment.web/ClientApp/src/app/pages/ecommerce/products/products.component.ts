import {Component, QueryList, ViewChildren} from '@angular/core';
import {DecimalPipe} from '@angular/common';
import {Observable} from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UntypedFormBuilder, UntypedFormGroup, FormArray, FormControl, Validators } from '@angular/forms';
import { GlobalComponent } from '../../../global-component';

// Products Services
import { ProductService } from "../../../core/services/rest-api.service";

// Range Slider
import { Options } from '@angular-slider/ngx-slider';

// Sweet Alert
import Swal from 'sweetalert2';

import {Products} from './data';
import {productModel} from './products.model';
import { AdvancedService } from './products.service';
import { NgbdProductsSortableHeader, SortEvent } from './products-sortable.directive';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
  providers: [AdvancedService, DecimalPipe]
})

/**
 * Products Components
 */
export class ProductsComponent {

  // bread crumb items
  breadCrumbItems!: Array<{}>;
  // Table data
  productList!: Observable<productModel[]>;
  total$: Observable<number>;
  @ViewChildren(NgbdProductsSortableHeader) headers!: QueryList<NgbdProductsSortableHeader>;
  url = GlobalComponent.API_URL;
  user = [];
  discountRates: number[] = [];
  contactsForm!: UntypedFormGroup;
  content!: productModel[];
  products!:any;

  constructor(private modalService: NgbModal,public service: AdvancedService, public ProductService: ProductService, private formBuilder: UntypedFormBuilder) {
    this.productList = service.countries$;
    this.total$ = service.total$;
  }

  ngOnInit(): void {
    /**
    * BreadCrumb
    */
     this.breadCrumbItems = [
      { label: 'Ecommerce' },
      { label: 'Products', active: true }
    ];

    // this.content = Products;
    // this.products = Object.assign([], this.content);   
    
    this.productList.subscribe(x => {
      this.products =  Object.assign([], x);   
    });
    
    // this.ProductService.getData().subscribe({
    //   next: data => {        
    //     const users =  JSON.parse(data);
    //     this.content = users.data;
    //     this.products = Object.assign([], this.content);  
    //   },
    //   error: err => {
    //     this.content = JSON.parse(err.error).message;
    //   }
    // });   

    
    /**
     * Form Validation
     */
    this.contactsForm = this.formBuilder.group({
      subItem: this.formBuilder.array([]),
    });
  }

  /**
  * Sort table data
  * @param param0 sort the column
  *
  */
   onSort({column, direction}: SortEvent) {
    // resetting other headers
    this.headers.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }

  // Price Slider
  minValue = 0;
  maxValue = 1000;
  pricevalue = 100;
  options: Options = {
    floor: 0,
    ceil: 1000
  };

  /**
   * Default Select2
   */
   multiDefaultOption = 'Watches';
   selectedAccount = 'This is a placeholder';
   Default = [
     { name: 'Watches' },
     { name: 'Headset' },
     { name: 'Sweatshirt' },
   ];

  /**
   * Rating Filter
   */
  changeRating(e:any, rate:any) {   
    if (e.target.checked && this.discountRates.length === 0) {
      this.productList.subscribe(x => {
        this.products = x.filter((product:any) => {     
          return product.rating > rate;
        });   
      }); 
      // this.products = this.content.filter((product:any) => {        
      //   return product.rating > rate;
      // });
    }
    else {
      this.productList.subscribe(x => {
        this.products = x.filter((product:any) => {     
          return product.discount !== rate;
        });   
      }); 
      // this.products = this.content.filter((product:any) => {        
      //   return product.discount !== rate;
      // }, this.discountRates);
    }
  }

  /**
   * Product Filtering  
   */
  changeProducts(e:any,name: any) {  
    this.productList.subscribe(x => {
      this.products = x.filter((product:any) => {     
        return product.category === name;
      });   
    }); 
    // this.products = this.products.filter((product:any) => {     
    //   return product.category === name;
    // });    
  }
   
  /**
  * Range Slider Wise Data Filter
  */
  valueChange(value: number, boundary: boolean): void {
    if (boundary) {
      this.minValue = value;
    } else {
      this.maxValue = value;
      this.productList.subscribe(x => {
        this.products = x.filter((product:any) => {     
          return product.price >= this.minValue && product.price <= this.maxValue;
        });   
      }); 
      // this.products = this.content.filter( (product:any) => {
      //   return product.price >= this.minValue && product.price <= this.maxValue;
      // });
    }
  }

  /**
  * Delete Model Open
  */
  deleteId: any;
  confirm(content: any,id:any) {
    this.deleteId = id;
    this.modalService.open(content, { centered: true });
  }
 
  // Delete Data
  deleteData(id:any) {
    if(id){
      this.ProductService.deleteData(id).subscribe({
        next: data => { },
        error: err => {
          this.content = JSON.parse(err.error).message;
        }
      });
      document.getElementById('p_'+id)?.remove();     
    }
    else{
      this.checkedValGet.forEach((item:any)=>{
        document.getElementById('p_'+ item)?.remove();      
      });
      (document.getElementById("selection-element") as HTMLElement).style.display = "none"
    }
  }

  // Check Box Checked Value Get
  checkedValGet: any[] = [];
  // Select Checkbox value Get
  onCheckboxChange(e: any) {
    var checkboxes:any = document.getElementsByName('checkAll');
    var checkedVal: any[] = [];
    var result
    for (var i = 0; i < checkboxes.length; i++) {
      if (checkboxes[i].checked) {
          result = checkboxes[i].value;
          checkedVal.push(result);   
      }
    }  
    this.checkedValGet = checkedVal
    var checkBoxCount:any = document.getElementById('select-content') as HTMLElement;
    checkBoxCount.innerHTML = checkedVal.length;
    checkedVal.length > 0 ? (document.getElementById("selection-element") as HTMLElement).style.display = "block":(document.getElementById("selection-element") as HTMLElement).style.display = "none";
  }

}
