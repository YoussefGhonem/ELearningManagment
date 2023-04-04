import { FormControl, FormGroup, UntypedFormBuilder } from '@angular/forms';
import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BaseComponent } from "@shared/base/base.component";
import { RegisterValidator } from "app/+auth/validators/register.validator";
import { CustomPdfViewerComponent } from "@libs/custom-pdf-viewer/custom-pdf-viewer.component";
import { ngbModalOptions } from "@shared/default-values";
import { NgbActiveModal, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { confirmPasswordValidator } from '@shared/custom-validators/confirm-password.validator';
import { CountryDropdownModel } from 'app/+users/models';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.scss']
})
export class SignupComponent extends BaseComponent implements OnInit {

  // Login Form
  signupForm!: FormGroup;
  fieldTextType!: boolean;
  fieldTextTypeForConfirm: boolean = false;
  // set the current year
  year: number = new Date().getFullYear();
  // Display Form or Success
  displayForm: boolean = true;
  verificationDocument: any;

  initializeSignaturePad: boolean = false;

  countries: CountryDropdownModel[];

  constructor(
    public override injector: Injector,
    private formBuilder: UntypedFormBuilder,
    private router: Router,
    public modalService: NgbModal,
    public activeModal: NgbActiveModal
  ) {
    super(injector);
    this.displayForm = true;
  }

  ngOnInit(): void {
    this.initForm();
    this.getCountries();
    this.getVendorTermsAndConditions();
  }

  initForm() {
    this.signupForm = this.formBuilder.group({
      firstName: new FormControl(null, RegisterValidator.userAccount.firstName),
      lastName: new FormControl(null, RegisterValidator.userAccount.lastName),
      email: new FormControl(null, RegisterValidator.userAccount.email),
      password: new FormControl(null, RegisterValidator.userAccount.password),
      confirmPassword: new FormControl(null, RegisterValidator.userAccount.confirmPassword('password')),
      callingCode: new FormControl(null, RegisterValidator.userAccount.callingCode),
      phoneNumber: new FormControl(null, RegisterValidator.userAccount.phoneNumber),
      signature: new FormControl(null, RegisterValidator.userAccount.signature),
      agreeTermsAndConditions: new FormControl(false, RegisterValidator.userAccount.agreeTermsAndConditions),
    });

    this.signupForm.valueChanges
      .subscribe(res => {
        console.log(this.signupForm.getRawValue());
      })
  }

  getCountries() {

  }
  showValidationPassword: boolean = false;
  onValidationChange() {
    this.showValidationPassword = true;
  }
  showValidationPasswordOption() {
    console.log("hi");
    if (!this.showValidationPassword) return;
    let input = document.getElementById("password-contain") as HTMLElement;
    input.style.display = "block";
  }

  getVendorTermsAndConditions() {

  }

  render() {
    let currentUrl = this.router.url;
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate([currentUrl]);
    });
  }

  onSubmit() {
    if (this.signupForm.invalid) {
      return;
    }
    let body = this.signupForm.getRawValue();
    body.phoneNumber = body.callingCode + '-' + body.phoneNumber;

  }

  /**
   * Password Hide/Show
   */
  toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
  }

  toggleFieldTextTypeForConfirmPassword() {
    this.fieldTextTypeForConfirm = !this.fieldTextTypeForConfirm;
  }

  saveSignature(signature: any) {
    if (this.initializeSignaturePad)
      this.signupForm.get('signature').patchValue(signature);
    this.initializeSignaturePad = true;
  }

  clearSignature() {
    this.signupForm.get('signature').patchValue(null);
    this.initializeSignaturePad = false;
  }

  openPreviewAndSign() {

  }

}
