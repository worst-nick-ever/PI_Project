$.widget('custom.collectionModifier',
       {
           options: {
               labelClasses: 'control-label col-md-2',
               inputType: 'text',
               inputClassses: 'form-control',
               required: false
           },
           _create: function () {
               var collectionName = this.options.name;
               var fields = this.options.fields;
               var self = this;

               var addBtn = $('<button class="colModAdd btn btn-default">Add</button>');
               addBtn.on('click', function () {
                   addBtn.before(self._createElement());
                   event.preventDefault();
               });

               this.element
                   .addClass('collectionModifier')
                   .attr('id', collectionName)
                   .append(addBtn);

               this.element.find('.colModRem').on('click', function (event) {
                   $(this).parents('.element').remove();
                   self._reindex();
                   event.preventDefault();
               });
           },

           _createElement: function () {
               var container = $('<div class="element"></div>');
               var fields = this.options.fields;
               var collectionName = this.options.name;
               var lastIndex = this._currentElementCount();

               for (var i = 0; i < fields.length; ++i) {
                   var current = $('<div class="form-group"></div>');
                   var labelClasses = fields[i].labelClasses ? fields[i].labelClasses : this.options.labelClasses;
                   var inputClasses = fields[i].inputClassses ? fields[i].inputClassses : this.options.inputClassses;
                   current.append('<label class="' + labelClasses + '">' + fields[i].label + '</label>');
                   var inputAttributes = 'name="' + collectionName + '[' + lastIndex + '].' + fields[i].inputName + '" class="' + inputClasses + '" ' + (fields[i].required ? 'data-val="true" data-val-required="' + fields[i].label + ' is required."' : '');
                   var input;
                   switch (fields[i].inputType) {
                       case 'textarea':
                           input = '<textarea ' + inputAttributes + '></textarea>';
                           break;
                       case 'date':
                           input = '<input type="text" ' + inputAttributes + ' />';
                           input = $(input);
                           input.datepicker({ dateFormat: 'mm/dd/yy' })
                           break;
                       case 'text':
                       default:
                           input = '<input type="text" ' + inputAttributes + ' />';
                           break;
                   }
                   current.append(input);
                   container.append(current);
               }

               var self = this;
               var removeBtn = $('<button class="colModRem btn btn-default">Remove</button>');
               removeBtn.on('click', function (event) {
                   container.remove();
                   self._reindex();
                   event.preventDefault();
               });

               container.append(removeBtn);

               return container;
           },

           _currentElementCount: function () {
               return this.element.find('.element').length;
           },

           _reindex: function () {
               this.element.find('.element').each(function (index) {
                   $(this).find('input,textarea').each(function (i, element) {
                       var currentName = $(element).attr('name');
                       $(element).attr('name', currentName.replace(/\[\d\]/, '[' + index + ']'));
                   });
               });
           }
       });