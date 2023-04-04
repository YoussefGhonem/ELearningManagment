import { Component, Injector, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { HttpService, NotificationService } from '@shared/services';
import { RolesEnum, UserModel } from "app/+auth/models";
import { takeUntil } from "rxjs/operators";
import { AuthService } from "app/+auth/service";

// Lib
export class ServiceLocator {
  static injector: Injector;
}

@Component({
  template: '',
})
export class BaseComponent implements OnDestroy {

  currentUser: UserModel | null = null;
  ngUnsubscribe = new Subject<void>();

  //#region Services
  public spinner: NgxSpinnerService;
  public httpService: HttpService;
  public notificationService: NotificationService;
  public authService: AuthService;

  //#endregion

  constructor(public injector: Injector) {
    this.spinner = this.injector.get(NgxSpinnerService);
    this.httpService = this.injector.get(HttpService);
    this.notificationService = this.injector.get(NotificationService);
    this.authService = this.injector.get(AuthService);

    // Set current user
    this.authService.currentUser$.pipe(takeUntil(this.ngUnsubscribe)).subscribe(user => {
      this.currentUser = user;
      console.log('New user => ', this.currentUser)
    });
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  get AdminRole(): string {
    return RolesEnum[RolesEnum.Admin];
  }

  get TeacherRole(): string {
    return RolesEnum[RolesEnum.Teacher];
  }

  get StudentRole(): string {
    return RolesEnum[RolesEnum.Student];
  }



}
