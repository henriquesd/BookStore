import {Component, OnInit, Input} from '@angular/core';
import {NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'ngbd-datepicker-popup',
  templateUrl: './datepicker-popup.html'
})

export class NgbdDatepickerPopup implements OnInit {
  model: NgbDateStruct;

  @Input() placeholder: string;

  constructor() { }

  ngOnInit() {
  }
}
