import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookService } from 'src/app/_services/book.service';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationDialogService } from 'src/app/_services/confirmation-dialog.service';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  public books: any;
  public listComplet: any;
  public searchTerm: string;
  public searchValueChanged: Subject<string> = new Subject<string>();

  constructor(private router: Router,
              private service: BookService,
              private toastr: ToastrService,
              private confirmationDialogService: ConfirmationDialogService) { }

  ngOnInit() {
    this.getValues();

    this.searchValueChanged.pipe(debounceTime(1000))
    .subscribe(() => {
      this.search();
    });
  }

  private getValues() {

    this.service.getBooks().subscribe(books => {
      this.books = books;
      this.listComplet = books;
    });
  }

  public addBook() {
    this.router.navigate(['/book']);
  }

  public editBook(bookId: number) {
    this.router.navigate(['/book/' + bookId]);
  }

  public deleteBook(bookId: number) {
    this.confirmationDialogService.confirm('Atention', 'Do you really want to delete this book?')
      .then(() =>
        this.service.deleteBook(bookId).subscribe(() => {
          this.toastr.success('The book has been deleted');
          this.getValues();
        },
          err => {
            this.toastr.error('Failed to delete the book.');
          }))
      .catch(() => '');
  }

  // Use the code below if you want to filter only using the front end;
  // public search() {
  //   const value = this.searchTerm.toLowerCase();
  //   this.books = this.listComplet.filter(
  //     book => book.name.toLowerCase().startsWith(value, 0) ||
  //       book.author.toLowerCase().startsWith(value, 0) ||
  //       book.description.toString().startsWith(value, 0) ||
  //       book.value.toString().startsWith(value, 0) ||
  //       book.publishDate.toString().startsWith(value, 0));
  // }

  public searchBooks() {
    this.searchValueChanged.next();
  }

  private search() {
    if (this.searchTerm !== '') {
      this.service.searchBooksWithCategory(this.searchTerm).subscribe(book => {
        this.books = book;
      }, error => {
        this.books = [];
      });
    } else {
      this.service.getBooks().subscribe(books => this.books = books);
    }
  }
}
