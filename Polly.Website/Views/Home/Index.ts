namespace Polly {

    export class Index {

        private elements: Elements;

        constructor() {
            this.elements = {
                $search: $("#search-button")
            };

            this.elements.$search.on('click', this.onClickSearchButton);
        }

        private onClickSearchButton = () => {
            this.elements.$search.closest('form').submit();
        }
    }

    interface Elements {
        $search: JQuery;
    }
}