import { takeUntil } from 'rxjs/operators';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { BaseComponent } from '@shared/base/base.component';
import { UsersValidator } from 'app/+users/validators/user.validator';
import { UsersController } from "app/+users/controllers";
import { RolesEnum } from "app/+auth/models";
import { CountryDropdownModel } from 'app/+users/models';

@Component({
  selector: 'app-create-user',
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.scss']
})
export class CreateUserComponent extends BaseComponent implements OnInit {

  @Input('role') role?: RolesEnum;
  form: FormGroup;
  userRolesEnum = RolesEnum;

  countries: CountryDropdownModel[];


  constructor(public override injector: Injector, public modalService: NgbActiveModal,
    private formBuilder: FormBuilder) {
    super(injector);
  }

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.form = this.formBuilder.group({
      firstName: new FormControl(null, UsersValidator.firstName),
      lastName: new FormControl(null, UsersValidator.lastName),
      callingCode: new FormControl(null, UsersValidator.callingCode),
      phoneNumber: new FormControl(null, UsersValidator.phoneNumber),
      email: new FormControl(null, UsersValidator.email),
    });
  }




  submit() {
    let body = this.form.getRawValue();
    body.phoneNumber = body.callingCode + '-' + body.phoneNumber;


    // this.httpService.POST(this.url, this.httpService.objectToFormData(body))
    //     .pipe(takeUntil(this.ngUnsubscribe))
    //     .subscribe(res => {
    //       this.modalService.close(true);
    //       this.notificationService.success('Vendor Created', 'Vendor is created successfully! 🎉');
    //     });


  }

}