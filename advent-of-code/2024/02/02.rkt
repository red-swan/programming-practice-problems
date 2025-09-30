#lang racket


(define (sign x)
  (cond
    [(< x 0) 'negative]
    [(< 0 x) 'positive]
    [else    'zero]))

(define (line->report line)
  (map string->number
       (string-split line " ")))

(define reports
  (with-input-from-file "02-sample.txt"
    (λ ()
      (for/list ([line (in-lines)])
        (line->report line)))))
                
(define (cumdiff xs)
  (let* ([n (length xs)]
         [k (- n 1)])
    (map - (rest xs) (take xs k))))


(define (is-safe report [allowable-errors 0] [sign #f])
  (cond
    [(empty? report)] #t)

(define (count-sign-switches xs)
  (let-values ([(count last-sign)
               (for/fold ([count 0]
                          [last-sign (sign (first xs))])
                         ([x (rest xs)])
                 (let ([next-sign (sign x)])
                   (if (equal? last-sign next-sign)
                       (values count        next-sign)
                       (values (add1 count) next-sign))))])
    count))
    

    

#|
(define (count-unsafe-jumps report)
  (let ([head (first report)]
        [tail (rest report)]
        [loop (λ (count head tail)
                (if
    (loop 0 head tail)))
  |#
; Scratch
(define asd (map cumdiff reports))
(map count-sign-switches asd)






























 