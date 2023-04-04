import { UnlockComponent } from '@shared/components/unlock/unlock.component';
import { UserAllowedActions } from 'app/+users/models';
import { ActivateComponent } from '@shared/components/activate/activate.component';
import { Component, EventEmitter, Injector, Input, OnInit, Output, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbActiveModal, NgbModal, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { BaseComponent } from '@shared/base/base.component';
import { UsersController } from 'app/+users/controllers/UsersController';
import { DeleteComponent } from '@shared/components/delete/delete.component';
import { ngbModalOptions } from '@shared/default-values';
import { DeactivateComponent } from '@shared/components/deactivate/deactivate.component';
import { TableDataAdapterService } from '@libs/primeng-table/services/table-data-adapter.service';
import { TableColumn } from '@libs/primeng-table/models/table-column.model';
import { debounceTime, takeUntil } from 'rxjs/operators';

@Component({
  selector: 'users-table',
  templateUrl: './users-table.component.html',
  styleUrls: ['./users-table.component.scss']
})
export class UsersTableComponent extends BaseComponent implements OnInit {
  @Input('form') form: FormGroup;
  allowedActions = UserAllowedActions;
  columns: TableColumn<any>[] = [];
  previewUser: any = null;
  users: any[];
  total: number = 0;
  constructor(
    public activeModal: NgbActiveModal,
    public modalService: NgbModal,
    public override injector: Injector,
    private offcanvasService: NgbOffcanvas) {
    super(injector);
  }

  ngOnChanges(): void {
    this.loadUsers()
  }

  getRoleClass(role: string) {
    switch (role) {
      case 'Admin':
        return 'success';
      case 'Teacher':
        return 'secondary';
      default:
        return 'warning';
    }
  }
  ngOnInit(): void {

    this.form.controls['searchWord'].valueChanges
      .pipe(debounceTime(500))
      .subscribe(res => {
        this.form.controls['pageNumber'].patchValue(1, { emitEvent: false });
        this.form.controls['pageSize'].patchValue(10, { emitEvent: false });
        this.loadUsers()

      });
  }

  public loadUsers() {
    let filters = this.form.getRawValue();
    this.httpService.GET(UsersController.Users, filters)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(res => {
        this.users = res?.data;
        this.total = res?.total;
        console.log("this.events", res);
      });
  }

  hasAllowedAction(user: any, action: UserAllowedActions): boolean {
    return user?.allowedActions?.includes(action);
  }

  activate(user: any) {
    const modalRef = this.modalService.open(ActivateComponent, {
      ...ngbModalOptions,
      windowClass: 'modal modal-success'
    });
    modalRef.componentInstance.title = user.name;
    modalRef.componentInstance.url = UsersController.Activate(user.id);

    modalRef
      .result
      .then((actionCompleted: boolean) => !actionCompleted || this.activeModal.close(true) || this.loadUsers())
      .catch(() => {
      });
  }

  unlock(user: any) {
    const modalRef = this.modalService.open(UnlockComponent, {
      ...ngbModalOptions,
      windowClass: 'modal modal-success'
    });
    modalRef.componentInstance.title = user.name;
    modalRef.componentInstance.url = UsersController.Unlock(user.id);
    modalRef
      .result
      .then((actionCompleted: boolean) => !actionCompleted || this.activeModal.close(true) || this.loadUsers())
      .catch(() => {
      });
  }

  deactivate(user: any) {
    const modalRef = this.modalService.open(DeactivateComponent, {
      ...ngbModalOptions,
      windowClass: 'modal modal-danger'
    });
    modalRef.componentInstance.title = user.name;
    modalRef.componentInstance.id = user.id;
    modalRef.componentInstance.url = UsersController.Deactivate(user.id);
    modalRef.componentInstance.reason = true;
    modalRef
      .result
      .then((actionCompleted: boolean) => !actionCompleted || this.activeModal.close(true) || this.loadUsers())
      .catch(() => {
      });
  }

  openOrderSlider(content: TemplateRef<any>, selectedUser: any) {
    this.previewUser = selectedUser;
    console.log(this.previewUser);
    this.offcanvasService.open(content, { position: 'end' });
  }

  updateUser() {
    this.loadUsers();
  }
}


